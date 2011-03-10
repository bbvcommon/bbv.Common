@echo off
set resultDir=..
set zipexe=..\zip.exe
set zip2exe=zip.exe
set librarydir=bbv.Common.Library
set librarytestdir=bbv.Common.Library.Test
set libraryspecificationsdir=bbv.Common.Library.Specification

echo "============================================================"
echo "Creating library project"
echo "============================================================"

cd %librarydir%
%zipexe% -r %resultDir%\bbv.Common.Library.zip *.*  -0 -u -x *.vscontent
cd %resultDir%
copy %librarydir%\bbv.Common.Library.vscontent .
%zip2exe% -r bbv.Common.Library.vsi bbv.Common.Library.zip bbv.Common.Library.vscontent  -0 -u

echo "============================================================"
echo "Creating test library project"
echo "============================================================"

cd %librarytestdir%
%zipexe% -r %resultDir%\bbv.Common.Library.Test.zip *.*  -0 -u -x *.vscontent
cd %resultDir%
copy %librarytestdir%\bbv.Common.Library.Test.vscontent .
%zip2exe% -r bbv.Common.Library.Test.vsi bbv.Common.Library.Test.zip bbv.Common.Library.Test.vscontent  -0 -u

echo "============================================================"
echo "Creating specifications library project"
echo "============================================================"

cd %libraryspecificationsdir%
%zipexe% -r %resultDir%\bbv.Common.Library.Specification.zip *.*  -0 -u -x *.vscontent
cd %resultDir%
copy %libraryspecificationsdir%\bbv.Common.Library.Specification.vscontent .
%zip2exe% -r bbv.Common.Library.Specification.vsi bbv.Common.Library.Specification.zip bbv.Common.Library.Specification.vscontent  -0 -u

del *.zip
del *.vscontent

echo "============================================================"
echo "Finished"
echo "============================================================"