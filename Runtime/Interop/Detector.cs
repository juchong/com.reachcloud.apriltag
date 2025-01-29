using System;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
using UnityEngine; // Required for Unity's Debug.Log

namespace AprilTag.Interop
{
    public sealed class Detector : SafeHandleZeroOrMinusOneIsInvalid
    {
        #region SafeHandle implementation

        Detector() : base(true)
        {
            UnityEngine.Debug.Log("[AprilTag]Detector created");
        }

        protected override bool ReleaseHandle()
        {
            UnityEngine.Debug.Log("[AprilTag]Releasing handle");
            _Destroy(handle);
            return true;
        }

        #endregion

        #region apriltag_detector struct representation

        [StructLayout(LayoutKind.Sequential)]
        internal struct InternalData
        {
            internal int nthreads;
            internal float quad_decimate;
            internal float quad_sigma;
            internal int refine_edges;
            internal double decode_sharpening;
            internal int debug;
            internal int min_cluster_pixels;
            internal int max_nmaxima;
            internal float critical_rad;
            internal float cos_critical_rad;
            internal float max_line_fit_mse;
            internal int min_white_black_diff;
            internal int deglitch;
            internal IntPtr tp;
            internal uint nedges;
            internal uint nsegments;
            internal uint nquads;
            internal IntPtr tag_families;
            internal IntPtr wp;
            // pthread_mutex_t mutex;
        }

        unsafe ref InternalData Data
            => ref Util.AsRef<InternalData>((void*)handle);

        #endregion

        #region Public properties and methods

        public int ThreadCount
        {
            get => Data.nthreads;
            set
            {
                UnityEngine.Debug.Log($"[AprilTag]Setting ThreadCount to {value}");
                Data.nthreads = value;
            }
        }

        public float QuadDecimate
        {
            get => Data.quad_decimate;
            set
            {
                UnityEngine.Debug.Log($"[AprilTag]Setting QuadDecimate to {value}");
                Data.quad_decimate = value;
            }
        }

        public float QuadSigma
        {
            get => Data.quad_sigma;
            set
            {
                UnityEngine.Debug.Log($"[AprilTag]Setting QuadSigma to {value}");
                Data.quad_sigma = value;
            }
        }

        public int RefineEdges
        {
            get => Data.refine_edges;
            set
            {
                UnityEngine.Debug.Log($"[AprilTag]Setting RefineEdges to {value}");
                Data.refine_edges = value;
            }
        }

        public double DecodeSharpening
        {
            get => Data.decode_sharpening;
            set
            {
                UnityEngine.Debug.Log($"[AprilTag]Setting DecodeSharpening to {value}");
                Data.decode_sharpening = value;
            }
        }

        public bool Debug
        {
            get => Data.debug != 0;
            set
            {
                UnityEngine.Debug.Log($"[AprilTag]Setting Debug to {value}");
                Data.debug = value ? 1 : 0;
            }
        }

        public unsafe ref TimeProfile TimeProfile
            => ref Util.AsRef<TimeProfile>((void*)Data.tp);

        public static Detector Create()
        {
            try
            {
                // Log the entry into the method
                UnityEngine.Debug.Log("[AprilTag]Create method called.");

                // Call the native _Create function to create a Detector instance
                UnityEngine.Debug.Log("[AprilTag]Calling _Create to initialize Detector...");
                Detector detector = _Create();

                // Check if the detector was created successfully
                if (detector == null || detector.IsInvalid)
                {
                    UnityEngine.Debug.LogError("[AprilTag]Failed to create Detector instance. _Create returned null or an invalid handle.");
                    return null;
                }

                UnityEngine.Debug.Log("[AprilTag]Detector instance successfully created.");
                return detector;
            }
            catch (Exception ex)
            {
                // Log any exceptions that occur during creation
                UnityEngine.Debug.LogError($"[AprilTag]An exception occurred in Create: {ex.Message}\n{ex.StackTrace}");
                return null;
            }
        }


        public void AddFamily(Family family)
        {
            UnityEngine.Debug.Log($"[AprilTag]Adding family: {family}");
            _AddFamilyBits(this, family, 2);
        }

        public void RemoveFamily(Family family)
        {
            UnityEngine.Debug.Log($"[AprilTag]Removing family: {family}");
            _RemoveFamily(this, family);
        }

        public DetectionArray Detect(ImageU8 image)
        {
            try
            {
                // Log the entry into the method
                //UnityEngine.Debug.Log("[AprilTag]Detect method called.");

                // Check if the image parameter is valid
                if (image == null)
                {
                    UnityEngine.Debug.LogError("[AprilTag]ImageU8 is null. Cannot perform detection.");
                    return null;
                }

                // Log the details of the input image (you might need to add properties to ImageU8 for debugging purposes)
                //UnityEngine.Debug.Log($"[AprilTag]ImageU8 provided: Width = {image.Width}, Height = {image.Height}");

                // Call the native _Detect function
                //UnityEngine.Debug.Log("[AprilTag]Calling _Detect...");
                DetectionArray detections = _Detect(this, image);

                // Check the result
                if (detections == null)
                {
                    UnityEngine.Debug.LogWarning("[AprilTag]No detections were returned by _Detect.");
                }
                else
                {
                    UnityEngine.Debug.Log($"[AprilTag]_Detect returned {detections.Length} detections.");
                }

                return detections;
            }
            catch (Exception ex)
            {
                // Log any exceptions that occur during detection
                UnityEngine.Debug.LogError($"[AprilTag]An exception occurred in Detect: {ex.Message}\n{ex.StackTrace}");
                return null;
            }
        }


        #endregion

        #region Unmanaged interface

        [DllImport(Config.DllName, EntryPoint = "apriltag_detector_create")]
        static extern Detector _Create();

        [DllImport(Config.DllName, EntryPoint = "apriltag_detector_destroy")]
        static extern void _Destroy(IntPtr detector);

        [DllImport(Config.DllName, EntryPoint = "apriltag_detector_add_family_bits")]
        static extern void _AddFamilyBits(Detector detector, Family family, int correctedBits);

        [DllImport(Config.DllName, EntryPoint = "apriltag_detector_remove_family")]
        static extern void _RemoveFamily(Detector detector, Family family);

        [DllImport(Config.DllName, EntryPoint = "apriltag_detector_detect")]
        static extern DetectionArray _Detect(Detector detector, ImageU8 image);

        #endregion
    }
} // namespace AprilTag.Interop
