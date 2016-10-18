using Leptonica.InteropNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Leptonica.Interop {

    public interface ILeptonicaNativeApi {

        #region Pix

        [RuntimeDllImport(LeptonicaConstants.DLLName, CallingConvention = CallingConvention.Cdecl, EntryPoint = LeptonicaConstants.MethodPixCreate)]
        unsafe IntPtr pixCreate(int with, int height, int depth);

        [RuntimeDllImport(LeptonicaConstants.DLLName, CallingConvention = CallingConvention.Cdecl, EntryPoint = LeptonicaConstants.MethodPixClone)]
        unsafe IntPtr pixClone(HandleRef pix);

        [RuntimeDllImport(LeptonicaConstants.DLLName, CallingConvention = CallingConvention.Cdecl, EntryPoint = LeptonicaConstants.MethodPixDestroy)]
        void pixDestroy(ref IntPtr pix);

        [RuntimeDllImport(LeptonicaConstants.DLLName, CallingConvention = CallingConvention.Cdecl, EntryPoint = LeptonicaConstants.MethodPixGetWidth)]
        int pixGetWidth(HandleRef pix);

        [RuntimeDllImport(LeptonicaConstants.DLLName, CallingConvention = CallingConvention.Cdecl, EntryPoint = LeptonicaConstants.MethodPixGetHeight)]
        int pixGetHeight(HandleRef pix);

        [RuntimeDllImport(LeptonicaConstants.DLLName, CallingConvention = CallingConvention.Cdecl, EntryPoint = LeptonicaConstants.MethodPixGetDepth)]
        int pixGetDepth(HandleRef pix);

        [RuntimeDllImport(LeptonicaConstants.DLLName, CallingConvention = CallingConvention.Cdecl, EntryPoint = LeptonicaConstants.MethodPixRead)]
        IntPtr pixRead(string filename);

        [RuntimeDllImport(LeptonicaConstants.DLLName, CallingConvention = CallingConvention.Cdecl, EntryPoint = LeptonicaConstants.MethodPixWrite)]
        int pixWrite(string filename, HandleRef handle, ImageSaveFormat format);

        [RuntimeDllImport(LeptonicaConstants.DLLName, CallingConvention = CallingConvention.Cdecl, EntryPoint = LeptonicaConstants.MethodPixClipRectangle)]
        IntPtr pixClipRectangle(HandleRef pixs, HandleRef box, IntPtr pboxc);

        #region pixconv.c

        [RuntimeDllImport(LeptonicaConstants.DLLName, CallingConvention = CallingConvention.Cdecl, EntryPoint = LeptonicaConstants.MethodPixConvertTo8)]
        IntPtr pixConvertTo8(HandleRef pixs, int flag);

        [RuntimeDllImport(LeptonicaConstants.DLLName, CallingConvention = CallingConvention.Cdecl, EntryPoint = LeptonicaConstants.MethodPixConvertRGBToGray)]
        IntPtr pixConvertRGBToGray(HandleRef pixs, float rwt, float gwt, float bwt);

        [RuntimeDllImport(LeptonicaConstants.DLLName, CallingConvention = CallingConvention.Cdecl, EntryPoint = LeptonicaConstants.MethodPixConvertTo1)]
        IntPtr pixConvertTo1(HandleRef pixs, int threshold);

        [RuntimeDllImport(LeptonicaConstants.DLLName, CallingConvention = CallingConvention.Cdecl, EntryPoint = LeptonicaConstants.MethodPixConvert1To8)]
        IntPtr pixConvert1To8(IntPtr pixd, HandleRef pixs, int whiteColor, int blackColor);

        [RuntimeDllImport(LeptonicaConstants.DLLName, CallingConvention = CallingConvention.Cdecl, EntryPoint = LeptonicaConstants.MethodPixConvertRGBToLuminance)]
        IntPtr pixConvertRGBToLuminance(HandleRef pixs);

        #endregion

        #region adaptmap.c

        [RuntimeDllImport(LeptonicaConstants.DLLName, CallingConvention = CallingConvention.Cdecl, EntryPoint = LeptonicaConstants.MethodPixBackgroundNormMorph)]
        IntPtr pixBackgroundNormMorph(HandleRef pix, IntPtr pixim, int reduction, int size, int backgroundColor);

        [RuntimeDllImport(LeptonicaConstants.DLLName, CallingConvention = CallingConvention.Cdecl, EntryPoint = LeptonicaConstants.MethodPixContrastNorm)]
        IntPtr pixContrastNorm(HandleRef pixd, HandleRef pixs, int sx, int sy, int mindiff, int smoothx, int smoothy);

        [RuntimeDllImport(LeptonicaConstants.DLLName, CallingConvention = CallingConvention.Cdecl, EntryPoint = LeptonicaConstants.MethodPixBackgroundNormRGBArraysMorph)]
        int pixBackgroundNormRGBArraysMorph(HandleRef pixs, IntPtr pixim, int reduction, int size, int backgroundColor, out IntPtr ppixr, out IntPtr ppixg, out IntPtr ppixb);

        [RuntimeDllImport(LeptonicaConstants.DLLName, CallingConvention = CallingConvention.Cdecl, EntryPoint = LeptonicaConstants.MethodPixApplyInvBackgroundRGBMap)]
        IntPtr pixApplyInvBackgroundRGBMap(HandleRef pixs, IntPtr pixmr, IntPtr pixmg, IntPtr pixmb, int sx, int sy);

        [RuntimeDllImport(LeptonicaConstants.DLLName, CallingConvention = CallingConvention.Cdecl, EntryPoint = LeptonicaConstants.MethodPixCleanBackgroundToWhite)]
        IntPtr pixCleanBackgroundToWhite(HandleRef pixs, IntPtr pixim, IntPtr pixg, float gamma, int blackSearch, int whileSearch);

        #endregion

        #region graymorph.c

        [RuntimeDllImport(LeptonicaConstants.DLLName, CallingConvention = CallingConvention.Cdecl, EntryPoint = LeptonicaConstants.MethodPixCloseGray)]
        IntPtr pixCloseGray(HandleRef pixs, int hsize, int vsize);
        
        #endregion

        #region convolve.c

        [RuntimeDllImport(LeptonicaConstants.DLLName, CallingConvention = CallingConvention.Cdecl, EntryPoint = LeptonicaConstants.MethodPixBlockconv)]
        IntPtr pixBlockconv(HandleRef pixs, int wc, int hc);

        #endregion

        #region dewarp4.c

        [RuntimeDllImport(LeptonicaConstants.DLLName, CallingConvention = CallingConvention.Cdecl, EntryPoint = LeptonicaConstants.MethodDewarpSinglePage)]
        int dewarpSinglePage(HandleRef pixs, DewrapThresh thresh, DewrapThresholding adaptive, DewrapOrientation use_both, out IntPtr pixd, IntPtr dewa, DewrapOptions debug);

        #endregion

        #region binarize.c

        [RuntimeDllImport(LeptonicaConstants.DLLName, CallingConvention = CallingConvention.Cdecl, EntryPoint = LeptonicaConstants.MethodPixOtsuAdaptiveThreshold)]
        int pixOtsuAdaptiveThreshold(HandleRef pix, int sx, int sy, int smoothx, int smoothy, float scorefract, out IntPtr ppixth, out IntPtr ppixd);

        [RuntimeDllImport(LeptonicaConstants.DLLName, CallingConvention = CallingConvention.Cdecl, EntryPoint = LeptonicaConstants.MethodPixSauvolaBinarize)]
        int pixSauvolaBinarize(HandleRef pix, int whsize, float factor, int addborder, out IntPtr ppixm, out IntPtr ppixsd, out IntPtr ppixth, out IntPtr ppixd);

        [RuntimeDllImport(LeptonicaConstants.DLLName, CallingConvention = CallingConvention.Cdecl, EntryPoint = LeptonicaConstants.MethodPixSauvolaBinarizeTiled)]
        int pixSauvolaBinarizeTiled(HandleRef pix, int whsize, float factor, int nx, int ny, out IntPtr ppixth, out IntPtr ppixd);
        
        #endregion

        #region scale.c

        [RuntimeDllImport(LeptonicaConstants.DLLName, CallingConvention = CallingConvention.Cdecl, EntryPoint = LeptonicaConstants.MethodPixScale)]
        IntPtr pixScale(HandleRef pixs, float scalex, float scaley);

        #endregion

        #region skew.c

        [RuntimeDllImport(LeptonicaConstants.DLLName, CallingConvention = CallingConvention.Cdecl, EntryPoint = LeptonicaConstants.MethodPixDeskewGeneral)]
        IntPtr pixDeskewGeneral(HandleRef pix, int redSweep, float sweepRange, float sweepDelta, int redSearch, int thresh, out float pAngle, out float pConf);

        [RuntimeDllImport(LeptonicaConstants.DLLName, CallingConvention = CallingConvention.Cdecl, EntryPoint = LeptonicaConstants.MethodPixFindSkewAndDeskew)]
        IntPtr pixFindSkewAndDeskew(HandleRef pix, int redsearch, out float pangle, out float pconf);
        
        #endregion

        #region rop.c

        [RuntimeDllImport(LeptonicaConstants.DLLName, CallingConvention = CallingConvention.Cdecl, EntryPoint = LeptonicaConstants.MethodPixTranslate)]
        IntPtr pixTranslate(IntPtr pixd, HandleRef pixs, int hshift, int vshift, ColorOptions incolor);

        #endregion

        #region rotate.c

        [RuntimeDllImport(LeptonicaConstants.DLLName, CallingConvention = CallingConvention.Cdecl, EntryPoint = LeptonicaConstants.MethodPixRotate)]
        IntPtr pixRotate(HandleRef pixs, float angle, RotationMethod type, RotationFill fillColor, int width, int heigh);

        [RuntimeDllImport(LeptonicaConstants.DLLName, CallingConvention = CallingConvention.Cdecl, EntryPoint = LeptonicaConstants.MethodPixRotateOrth)]
        IntPtr pixRotateOrth(HandleRef pixs, int quads);

        #endregion

        #region enhance.c

        [RuntimeDllImport(LeptonicaConstants.DLLName, CallingConvention = CallingConvention.Cdecl, EntryPoint = LeptonicaConstants.MethodPixUnsharpMasking)]
        IntPtr pixUnsharpMasking(HandleRef pixs, int halfwidth, float fract);
        
        #endregion

        #region edge.c

        [RuntimeDllImport(LeptonicaConstants.DLLName, CallingConvention = CallingConvention.Cdecl, EntryPoint = LeptonicaConstants.MethodPixSobelEdgeFilter)]
        IntPtr pixSobelEdgeFilter(HandleRef pixs, EdgeDirection orientflag);
        
        #endregion

        #region boxbasic.c

        [RuntimeDllImport(LeptonicaConstants.DLLName, CallingConvention = CallingConvention.Cdecl, EntryPoint = LeptonicaConstants.MethodBoxCreate)]
        unsafe IntPtr boxCreate(int x, int y, int w, int h);

        [RuntimeDllImport(LeptonicaConstants.DLLName, CallingConvention = CallingConvention.Cdecl, EntryPoint = LeptonicaConstants.MethodBoxClone)]
        unsafe IntPtr boxClone(HandleRef box);

        [RuntimeDllImport(LeptonicaConstants.DLLName, CallingConvention = CallingConvention.Cdecl, EntryPoint = LeptonicaConstants.MethodBoxDestroy)]
        void boxDestroy(ref IntPtr pbox);

        [RuntimeDllImport(LeptonicaConstants.DLLName, CallingConvention = CallingConvention.Cdecl, EntryPoint = LeptonicaConstants.MethodBoxGetGeometry)]
        int boxGetGeometry(HandleRef box, out int px, out int py, out int pw, out int ph);

        [RuntimeDllImport(LeptonicaConstants.DLLName, CallingConvention = CallingConvention.Cdecl, EntryPoint = LeptonicaConstants.MethodBoxaGetBox)]
        IntPtr boxaGetBox(HandleRef boxa, int index, L_ACCESS_FLAG accessflag);

        [RuntimeDllImport(LeptonicaConstants.DLLName, CallingConvention = CallingConvention.Cdecl, EntryPoint = LeptonicaConstants.MethodBoxaDestroy)]
        void boxaDestroy(ref IntPtr pboxa);

        #endregion

        #region morphseq.c

        [RuntimeDllImport(LeptonicaConstants.DLLName, CallingConvention = CallingConvention.Cdecl, EntryPoint = LeptonicaConstants.MethodPixMorphSequence)]
        IntPtr pixMorphSequence(HandleRef pixs, string sequence, int dispsep);

        #endregion

        #region conncomp.c

        [RuntimeDllImport(LeptonicaConstants.DLLName, CallingConvention = CallingConvention.Cdecl, EntryPoint = LeptonicaConstants.MethodPixConnComp)]
        IntPtr pixConnComp(HandleRef pixs, out IntPtr pixxa, int connectivity);
        
        #endregion

        #region boxfunc2.c

        [RuntimeDllImport(LeptonicaConstants.DLLName, CallingConvention = CallingConvention.Cdecl, EntryPoint = LeptonicaConstants.MethodBoxaSort)]
        IntPtr boxaSort(HandleRef boxa, L_SORT_BY sorttype, L_SORT sortorder, IntPtr pnaindex);
        
        #endregion


        #endregion

        [RuntimeDllImport(LeptonicaConstants.DLLName, CallingConvention = CallingConvention.Cdecl, EntryPoint = LeptonicaConstants.MethodPixThresholdToBinary)]
        IntPtr pixThresholdToBinary(HandleRef pixs, int color);

        [RuntimeDllImport(LeptonicaConstants.DLLName, CallingConvention = CallingConvention.Cdecl, EntryPoint = LeptonicaConstants.MethodPixFindSkew)]
        int pixFindSkew(HandleRef pixs, out double angle, out double confidence);

        [RuntimeDllImport(LeptonicaConstants.DLLName, CallingConvention = CallingConvention.Cdecl, EntryPoint = LeptonicaConstants.MethodPixRotateAMGray)]
        IntPtr pixRotateAMGray(HandleRef pixs, double radians, int color);

        [RuntimeDllImport(LeptonicaConstants.DLLName, CallingConvention = CallingConvention.Cdecl, EntryPoint = LeptonicaConstants.MethodPixErodeGray)]
        IntPtr pixErodeGray(HandleRef pixs, int a, int b);
 
        [RuntimeDllImport(LeptonicaConstants.DLLName, CallingConvention = CallingConvention.Cdecl, EntryPoint = LeptonicaConstants.MethodPixThresholdToValue)]
        IntPtr pixThresholdToValue(IntPtr ptr, HandleRef pixs, int b, int w);

        [RuntimeDllImport(LeptonicaConstants.DLLName, CallingConvention = CallingConvention.Cdecl, EntryPoint = LeptonicaConstants.MethodPixInvert)]
        IntPtr pixInvert(HandleRef pixs, HandleRef pixd);

        [RuntimeDllImport(LeptonicaConstants.DLLName, CallingConvention = CallingConvention.Cdecl, EntryPoint = LeptonicaConstants.MethodPixAddGray)]
        IntPtr pixAddGray(IntPtr ptr, HandleRef pixs, HandleRef pixd);

        [RuntimeDllImport(LeptonicaConstants.DLLName, CallingConvention = CallingConvention.Cdecl, EntryPoint = LeptonicaConstants.MethodPixOpenGray)]
        IntPtr pixOpenGray(HandleRef pixs, int a, int b);

        [RuntimeDllImport(LeptonicaConstants.DLLName, CallingConvention = CallingConvention.Cdecl, EntryPoint = LeptonicaConstants.MethodPixCombineMasked)]
        int pixCombineMasked(HandleRef pixs, HandleRef pixd, HandleRef pixt);

        [RuntimeDllImport(LeptonicaConstants.DLLName, CallingConvention = CallingConvention.Cdecl, EntryPoint = LeptonicaConstants.MethodPixSeedfillBinary)]
        IntPtr pixSeedfillBinary(IntPtr ptr, HandleRef pixd, HandleRef pixt, int xtr);

        [RuntimeDllImport(LeptonicaConstants.DLLName, CallingConvention = CallingConvention.Cdecl, EntryPoint = LeptonicaConstants.MethodPixExpandBinaryPower2)]
        IntPtr pixExpandBinaryPower2(HandleRef pixd, int xtr);

        [RuntimeDllImport(LeptonicaConstants.DLLName, CallingConvention = CallingConvention.Cdecl, EntryPoint = LeptonicaConstants.MethodPixSubtract)]
        IntPtr pixSubtract(HandleRef pixd, HandleRef pixs1, HandleRef pixs2);

        [RuntimeDllImport(LeptonicaConstants.DLLName, CallingConvention = CallingConvention.Cdecl, EntryPoint = LeptonicaConstants.MethodPixGammaTRC)]
        IntPtr pixGammaTRC(HandleRef pixd, HandleRef pixs, float gamma, int minVal, int maxVal);

        [RuntimeDllImport(LeptonicaConstants.DLLName, CallingConvention = CallingConvention.Cdecl, EntryPoint = LeptonicaConstants.MethodPixDitherTo2bpp)]
        IntPtr pixDitherTo2bpp(HandleRef pixs, int cmapflag);

        [RuntimeDllImport(LeptonicaConstants.DLLName, CallingConvention = CallingConvention.Cdecl, EntryPoint = LeptonicaConstants.MethodPixThresholdTo4bpp)]
        IntPtr pixThresholdTo4bpp(HandleRef pixs, int nlevels, int cmapflag);

        [RuntimeDllImport(LeptonicaConstants.DLLName, CallingConvention = CallingConvention.Cdecl, EntryPoint = LeptonicaConstants.MethodPixBackgroundNormFlex)]
        IntPtr pixBackgroundNormFlex(HandleRef pixs, int sx, int sy, int smoothx, int smoothy, int delta);

        [RuntimeDllImport(LeptonicaConstants.DLLName, CallingConvention = CallingConvention.Cdecl, EntryPoint = LeptonicaConstants.MethodPixScaleSmooth)]
        IntPtr pixScaleSmooth(HandleRef pixs, float scaleX, float scaleY);

        [RuntimeDllImport(LeptonicaConstants.DLLName, CallingConvention = CallingConvention.Cdecl, EntryPoint = LeptonicaConstants.MethodPixLocalExtrema)]
        int pixLocalExtrema(HandleRef pixs, int maximin, int minmax, out IntPtr ppixmin, out IntPtr ppixmax);

        [RuntimeDllImport(LeptonicaConstants.DLLName, CallingConvention = CallingConvention.Cdecl, EntryPoint = LeptonicaConstants.MethodPixExpandBinaryReplicate)]
        IntPtr pixExpandBinaryReplicate(HandleRef pixs, int factor);

        [RuntimeDllImport(LeptonicaConstants.DLLName, CallingConvention = CallingConvention.Cdecl, EntryPoint = LeptonicaConstants.MethodPixSeedfillGrayBasin)]
        IntPtr pixSeedfillGrayBasin(HandleRef pixb, HandleRef pixm, int delta, int connectivity);

        [RuntimeDllImport(LeptonicaConstants.DLLName, CallingConvention = CallingConvention.Cdecl, EntryPoint = LeptonicaConstants.MethodPixExtendByReplication)]
        IntPtr pixExtendByReplication(HandleRef pixs, int addw, int addh);

        [RuntimeDllImport(LeptonicaConstants.DLLName, CallingConvention = CallingConvention.Cdecl, EntryPoint = LeptonicaConstants.MethodPixGetInvBackgroundMap)]
        IntPtr pixGetInvBackgroundMap(HandleRef pixs, int backgroundValue, int smoothx, int smoothy);

        [RuntimeDllImport(LeptonicaConstants.DLLName, CallingConvention = CallingConvention.Cdecl, EntryPoint = LeptonicaConstants.MethodPixApplyInvBackgroundGrayMap)]
        IntPtr pixApplyInvBackgroundGrayMap(HandleRef pixs, HandleRef pixm, int sx, int sy);

        [RuntimeDllImport(LeptonicaConstants.DLLName, CallingConvention = CallingConvention.Cdecl, EntryPoint = LeptonicaConstants.MethodPixGammaTRCMasked)]
        IntPtr pixGammaTRCMasked(IntPtr pixd, HandleRef pixs, IntPtr pixm, float gamma, int minvalue, int maxvalue);

        [RuntimeDllImport(LeptonicaConstants.DLLName, CallingConvention = CallingConvention.Cdecl, EntryPoint = LeptonicaConstants.MethodPixTophat)]
        IntPtr pixTophat(HandleRef pixs, int hsize, int vsize, L_TOPHAT type);

        [RuntimeDllImport(LeptonicaConstants.DLLName, CallingConvention = CallingConvention.Cdecl, EntryPoint = LeptonicaConstants.MethodPixBackgroundNorm)]
        IntPtr pixBackgroundNorm(HandleRef pixs, IntPtr pixim, IntPtr pixg, int sx, int sy, int thresh, int minCount, int backgroundValue, int smoothx, int smoothy);

        [RuntimeDllImport(LeptonicaConstants.DLLName, CallingConvention = CallingConvention.Cdecl, EntryPoint = LeptonicaConstants.MethodPixGetPixel)]
        int pixGetPixel(HandleRef pix, int x, int y, out int pixelValue);

        [RuntimeDllImport(LeptonicaConstants.DLLName, CallingConvention = CallingConvention.Cdecl, EntryPoint = LeptonicaConstants.MethodPixBackgroundNormSimple)]
        IntPtr pixBackgroundNormSimple(HandleRef pixs, IntPtr pixd, IntPtr pixth);

        [RuntimeDllImport(LeptonicaConstants.DLLName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "pixaDestroy")]
        void pixaDestroy(ref IntPtr pixa);

        [RuntimeDllImport(LeptonicaConstants.DLLName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "pixaDisplay")]
        IntPtr pixaDisplay(HandleRef pixa, int width, int height);

        [RuntimeDllImport(LeptonicaConstants.DLLName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "pixaSelectBySize")]
        IntPtr pixaSelectBySize(HandleRef pixa, int width, int height, L_SELECT type, L_SELECT_CONDITON condition, int pchanged);
        /*
            public const string MethodPixFindSkew           = "pixFindSkew";--
            public const string MethodPixErodeGray          = "pixErodeGray";--
            public const string MethodPixThresholdToValue   = "pixThresholdToValue";--
            public const string MethodPixThresholdToBinary  = "pixThresholdToBinary";--
            public const string MethodPixInvert             = "pixInvert";--
            public const string MethodPixAddGray            = "pixAddGray";--
            public const string MethodPixOpenGray           = "pixOpenGray";--
            public const string MethodPixCombineMasked      = "pixCombineMasked";--
            public const string MethodPixRotateAMGray       = "pixRotateAMGray";--
            public const string MethodPixSeedfillBinary     = "pixSeedfillBinary";--
            public const string MethodPixExpandBinaryPower2 = "pixExpandBinaryPower2";--
            public const string MethodPixSubstract          = "pixSubtract";--
            public const string MethodPixDitherTo2bpp       = "pixDitherTo2bpp";--
            public const string MethodPixThresholdTo4bpp    = "pixThresholdTo4bpp";--
            public const string MethodPixBackgroundNormFlex = "pixBackgroundNormFlex";

         
         */
    }

    public class LeptonicaConstants {

        public const string DLLName = "liblept172";

        #region Native Entry Points
        //pix.c
        public const string MethodPixCreate         = "pixCreate";
        public const string MethodPixClone          = "pixClone";
        public const string MethodPixDestroy        = "pixDestroy";
        public const string MethodPixGetWidth       = "pixGetWidth";
        public const string MethodPixGetHeight      = "pixGetHeight";
        public const string MethodPixGetDepth       = "pixGetDepth";
        public const string MethodPixRead           = "pixRead";
        public const string MethodPixWrite          = "pixWrite";
        public const string MethodPixClipRectangle  = "pixClipRectangle";
        public const string MethodPixGetPixel       = "pixGetPixel";    
        //pixconv.c
        public const string MethodPixConvertTo8             = "pixConvertTo8";
        public const string MethodPixConvertRGBToGray       = "pixConvertRGBToGray";
        public const string MethodPixConvertTo1             = "pixConvertTo1";
        public const string MethodPixConvert1To8            = "pixConvert1To8";
        public const string MethodPixConvertRGBToLuminance  = "pixConvertRGBToLuminance";
        //binarize.c
        public const string MethodPixBackgroundNormMorph    = "pixBackgroundNormMorph";
        public const string MethodPixSauvolaBinarize        = "pixSauvolaBinarize";
        public const string MethodPixSauvolaBinarizeTiled   = "pixSauvolaBinarizeTiled";
        //adaptmap.c
        public const string MethodPixOtsuAdaptiveThreshold          = "pixOtsuAdaptiveThreshold";
        public const string MethodPixContrastNorm                   = "pixContrastNorm";
        public const string MethodPixBackgroundNormRGBArraysMorph   = "pixBackgroundNormRGBArraysMorph";
        public const string MethodPixApplyInvBackgroundRGBMap       = "pixApplyInvBackgroundRGBMap";
        public const string MethodPixCleanBackgroundToWhite         = "pixCleanBackgroundToWhite";
        //scale.c
        public const string MethodPixScale = "pixScale";
        public const string MethodPixScaleSmooth = "pixScaleSmooth";
        //skew.c
        public const string MethodPixDeskewGeneral      = "pixDeskewGeneral";
        public const string MethodPixFindSkewAndDeskew  = "pixFindSkewAndDeskew";
        //
        public const string MethodPixFindSkew                   = "pixFindSkew";
        public const string MethodPixErodeGray                  = "pixErodeGray";
        public const string MethodPixThresholdToValue           = "pixThresholdToValue";
        public const string MethodPixThresholdToBinary          = "pixThresholdToBinary";
        public const string MethodPixInvert                     = "pixInvert";
        public const string MethodPixAddGray                    = "pixAddGray";
        public const string MethodPixOpenGray                   = "pixOpenGray";
        public const string MethodPixCombineMasked              = "pixCombineMasked";
        public const string MethodPixRotateAMGray               = "pixRotateAMGray";
        //
        public const string MethodPixSeedfillBinary             = "pixSeedfillBinary";
        public const string MethodPixExpandBinaryPower2         = "pixExpandBinaryPower2";
        public const string MethodPixSubtract                   = "pixSubtract";
        public const string MethodPixGammaTRC                   = "pixGammaTRC";
        public const string MethodPixDitherTo2bpp               = "pixDitherTo2bpp";
        public const string MethodPixThresholdTo4bpp            = "pixThresholdTo4bpp";
        public const string MethodPixBackgroundNormFlex         = "pixBackgroundNormFlex";
        public const string MethodPixLocalExtrema               = "pixLocalExtrema";
        public const string MethodPixExpandBinaryReplicate      = "pixExpandBinaryReplicate";
        public const string MethodPixSeedfillGrayBasin          = "pixSeedfillGrayBasin";
        public const string MethodPixExtendByReplication        = "pixExtendByReplication";
        public const string MethodPixGetInvBackgroundMap        = "pixGetInvBackgroundMap";
        public const string MethodPixApplyInvBackgroundGrayMap  = "pixApplyInvBackgroundGrayMap";
        public const string MethodPixGammaTRCMasked             = "pixGammaTRCMasked";
        public const string MethodPixTophat                     = "pixTophat";
        public const string MethodPixBackgroundNorm             = "pixBackgroundNorm";
        public const string MethodPixBackgroundNormSimple       = "pixBackgroundNormSimple";
        //rotate.c
        public const string MethodPixRotate     = "pixRotate";
        public const string MethodPixRotateOrth = "pixRotateOrth";
        //enhance.c
        public const string MethodPixUnsharpMasking = "pixUnsharpMasking";
        //edge.c
        public const string MethodPixSobelEdgeFilter = "pixSobelEdgeFilter";
        //boxbasic.c
        public const string MethodBoxCreate         = "boxCreate";
        public const string MethodBoxClone          = "boxClone";
        public const string MethodBoxDestroy        = "boxDestroy";
        public const string MethodBoxGetGeometry    = "boxGetGeometry";
        public const string MethodBoxaGetBox        = "boxaGetBox";
        public const string MethodBoxaDestroy       = "boxaDestroy";
        //dewarp4.c
        public const string MethodDewarpSinglePage = "dewarpSinglePage";
        //rop.c
        public const string MethodPixTranslate = "pixTranslate";
        //graymorph.c
        public const string MethodPixCloseGray = "pixCloseGray";
        //convolve.c
        public const string MethodPixBlockconv = "pixBlockconv";
        //morphseq.c
        public const string MethodPixMorphSequence = "pixMorphSequence";
        //conncomp.c
        public const string MethodPixConnComp = "pixConnComp";
        //boxfunc2.c
        public const string MethodBoxaSort = "boxaSort";
        #endregion 
    }

    unsafe public static class LeptonicaNativeApi {

        private static ILeptonicaNativeApi native;

        public static ILeptonicaNativeApi Native {
            get {
                if(native == null) {
                    NativeInit();
                }
                return native;
            }
        }

        public static void NativeInit() {
            if(native == null) {
                native = InteropRuntimeImplementer.CreateInstance<ILeptonicaNativeApi>();
            }
        }
    }
}
