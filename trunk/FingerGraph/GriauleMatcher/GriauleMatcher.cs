/*
 * Griaule biometrics matcher
 * (+ templates cache)
 * 
 * Last update : 31.03.2010
 */

#region General usings
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
#endregion

#region FingerGraph usings
using FingerGraphDB;
using FingerGraph.Matcher;
using FingerGraph.Properties;
#endregion

#region GriauleFingerprintLibrary usings
using GriauleFingerprintLibrary;
using GriauleFingerprintLibrary.Exceptions;
using GriauleFingerprintLibrary.DataTypes;
#endregion

namespace FingerGraph.GriauleMatcher
{
    /// <summary>
    /// Griaule biometrics matcher
    /// Now with cache of templates ^_^
    /// 
    /// NOTE: You have to install Griaule Biometrics SDK
    /// </summary>
    class GriauleMatcher : IMatcher
    {
        #region Fields
        // Fingerprint core (very helpful stuff)
        private FingerprintCore fingerPrintCore;

        // templates cache (for optimization)
        private Dictionary<FingerprintRawImage, FingerprintTemplate> templateCache;        
        #endregion

        #region constructor and destructor
        /// <summary>
        /// Constructor and initializator
        /// </summary>
        public GriauleMatcher()
        {            
            fingerPrintCore = new FingerprintCore();

            try
            {
                fingerPrintCore.Initialize();
            }
            catch
            {
                throw new Exception("Unable to initialize fingerPrintCore");
            }

            templateCache = new Dictionary<FingerprintRawImage,FingerprintTemplate>();
        }

        /// <summary>
        /// Destructor with fingerPrintCore finalization
        /// </summary>
        ~GriauleMatcher()
        {
            try
            {
                fingerPrintCore.Finalizer();
            }
            catch
            {
                throw new Exception("Unable to finalize fingerPrintCore");
            }
        }
        #endregion

        #region IMatcher interface methods
        /// <summary>
        /// Identification
        /// </summary>
        /// <param name="fingerprint">image of fingerprint to identificate </param>
        /// <param name="cards"></param>
        /// <returns></returns>
        public FingerprintCard Identify(Image fingerprint, FingerprintCard[] cards) 
        {            
            int numberOfVerified = 0;
            FingerprintCard identifiedCard = null;

            #region paranoia section
            if (fingerprint == null)
                throw new ArgumentNullException("fingerprint");
            if (cards == null)
                throw new ArgumentNullException("cards");
            #endregion

            foreach (FingerprintCard card in cards)
            {
                #region paranoia section
                if (card == null)
                    throw new ArgumentNullException("card");

                if (card.FingerprintList == null)
                    throw new ArgumentNullException("card.FingerprintList");

                if (card.FingerprintList.Count == 0)
                    throw new ArgumentOutOfRangeException("card.FingerprintList.Count");
                #endregion               

                // If successfuly verified
                if (Verify(fingerprint, card))
                {
                    identifiedCard = card;
                    numberOfVerified++;
                }
            }

            // Decide what to return
            switch (numberOfVerified)
            {
                case 0: return null;
                case 1: return identifiedCard;
                default: throw new Exception("Oh shi~! Here is some critical security error!" +
                    "For current image successfuly verified more than one fingercard!");
            }
        }

        /// <summary>
        /// Verification
        /// </summary>
        /// <param name="fingerprint">image of fingerprint to verify</param>
        /// <param name="card">card</param>
        /// <returns>true if success, otherwise false</returns>
        public bool Verify(Image fingerprint, FingerprintCard card) 
        {
            // The best verification score reached
            int maxScore = 0;

            #region paranoia section
            if (fingerprint == null)
                throw new ArgumentNullException("fingerprint");

            if (card == null)
                throw new ArgumentNullException("card");

            if (card.FingerprintList == null)
                throw new ArgumentNullException("card.FingerprintList");

            if (card.FingerprintList.Count == 0)
                throw new ArgumentOutOfRangeException("card.FingerprintList.Count");
            #endregion

            FingerprintTemplate templateToVerify;
            FingerprintRawImage rawImageToVerify = RawImageFromImage(fingerprint);            
            templateToVerify = ExtractTemplate(rawImageToVerify);

            foreach (Image tempImage in card.FingerprintList)
            {
                FingerprintTemplate templateTemp;
                FingerprintRawImage rawImageTemp = RawImageFromImage(tempImage);
                templateTemp = ExtractTemplate(rawImageTemp);

                int result;
                fingerPrintCore.Verify(templateToVerify, templateTemp, out result);
                                
                if (result > maxScore)
                    maxScore = result;
            }

            // If success...
            if (maxScore >= Settings.Default.GriauleMatcherMinScoreToSuccessVerify)
                return true;

            // If failed...
            return false;
        }
        #endregion

        #region Some additional stuff
        /// <summary>
        /// Set fingerPrintCore verify parameters
        /// 
        /// NOTE: Read Griaule biometrics SDK manual first!
        /// </summary>
        /// <param name="verifyThreshold">threshold</param>
        /// <param name="verifyRotationTolerance">rotation tolerance</param>
        public void SetVerifyParameters(int verifyThreshold, int verifyRotationTolerance)
        {
            fingerPrintCore.SetVerifyParameters(verifyThreshold, verifyRotationTolerance);
        }

        /// <summary>
        /// Extract template from rawImage
        /// </summary>
        /// <param name="rawImage">1-byte per pixel raw image</param>
        /// <returns>template if success, otherwise it throws exception</returns>
        private FingerprintTemplate ExtractTemplate(FingerprintRawImage rawImage)
        {
            FingerprintTemplate tmpTemplate;

            // if template already stored in cache
            if (templateCache.TryGetValue(rawImage, out tmpTemplate))
            {
                return tmpTemplate;
            }

            // otherwise extract template and store it
            try
            {
                tmpTemplate = null;
                fingerPrintCore.Extract(rawImage, ref tmpTemplate);
                templateCache.Add(rawImage, tmpTemplate);

                return tmpTemplate;
            }
            catch
            {
                throw new Exception("Unable to extract template!");
            }
        }

        /// <summary>
        /// Convert System.Drawing.Image to FingerprintTemplate
        /// </summary>
        /// <param name="image">image to convert</param>
        /// <returns>raw image from image</returns>
        private FingerprintRawImage RawImageFromImage(Image image)
        {
            FingerprintRawImage retRawImage = new FingerprintRawImage();
            Bitmap bitmapTemp = image as Bitmap;

            retRawImage.rawImage = new byte[bitmapTemp.Width * bitmapTemp.Height];

            for (int y = 0; y < bitmapTemp.Height; y++)
                for (int x = 0; x < bitmapTemp.Width; x++)
                {
                    retRawImage.rawImage[y * bitmapTemp.Width + x] = bitmapTemp.GetPixel(x, y).R;
                }

            retRawImage.width = bitmapTemp.Width;
            retRawImage.height = bitmapTemp.Height;

            // Oh, shi~  This f*cken SDK use resolution of scaner, but not image >_<
            retRawImage.res = Settings.Default.scanerResolution;

            return retRawImage;
        }
        #endregion
    }
}
