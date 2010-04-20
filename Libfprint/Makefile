all:
	swig -csharp img_capture.i
	gcc -c -fpic img_capture.c img_capture_wrap.c -l fprint
	gcc -shared img_capture.o img_capture_wrap.o -o libimg_capture.so
	mono-csc *.cs -o mono.exe
