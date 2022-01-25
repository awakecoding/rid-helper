using System;
using System.IO;
using System.Runtime.InteropServices;

// .NET RID Catalog:
// https://docs.microsoft.com/en-us/dotnet/core/rid-catalog

// RuntimeInformation Class
// https://docs.microsoft.com/en-us/dotnet/api/system.runtime.interopservices.runtimeinformation?view=net-6.0

// Architecture
// https://docs.microsoft.com/en-us/dotnet/api/system.runtime.interopservices.architecture?view=net-6.0

// RID Format: [os].[version]-[architecture]-[additional qualifiers]
// [os] is the operating/platform system moniker. For example, ubuntu.
// [version] is the operating system version in the form of a dot-separated (.) version number. For example, 15.10.
// [architecture] is the processor architecture. For example: x86, x64, arm, or arm64.
// [additional qualifiers] further differentiate different platforms. For example: aot.

namespace RidHelper
{
    static class RidHelper
    {
        public static string GetRuntimeArchName()
        {
            Architecture arch = System.Runtime.InteropServices.RuntimeInformation.ProcessArchitecture;

            switch (arch) {
                case Architecture.X86:
                    return "x86";
                case Architecture.X64:
                    return "x64";
                case Architecture.Arm:
                    return "arm";
                case Architecture.Arm64:
                    return "arm64";
            }

            return "unknown";
        }

        public static bool IsWindows()
        {
            return System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
        }

        public static bool IsMacOS()
        {
            return System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
        }

        public static bool IsLinux()
        {
            return System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
        }

        public static string GetRuntimeOSPlatformName()
        {
            if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) {
                return "win";
            } else if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.OSX)) {
                return "osx";
            } else if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.Linux)) {
                return "linux";
            }

            // TODO: Android and iOS

            return "unknown";
        }

        public static string GetRuntimeIdentifier()
        {
            string arch = GetRuntimeArchName();
            string os = GetRuntimeOSPlatformName();
            string rid = String.Format("{0}-{1}", os, arch);
            return rid;
        }

        public static string GetRuntimeNativePath(bool absolute)
        {
            string rid = GetRuntimeIdentifier();
            string path = Path.Combine("runtimes", rid, "native");

            if (absolute) {
                string cwd = Directory.GetCurrentDirectory();
                path = Path.Combine(cwd, path);
            }

            return path;
        }

        public static string GetSharedLibraryExtension(bool withDot)
        {
            string ext = "so";

            if (IsWindows()) {
                ext = "dll";
            } else if (IsMacOS()) {
                ext = "dylib";
            }

            if (withDot) {
                ext = "." + ext;
            }

            return ext;
        }

        public static string GetNativeLibraryName(string libraryName)
        {
            if (!IsWindows()) {
                if (!libraryName.StartsWith("lib")) {
                    libraryName = "lib" + libraryName;
                }
            }

            if (!Path.HasExtension(libraryName)) {
                libraryName = libraryName + GetSharedLibraryExtension(true);
            }

            return libraryName;
        }

        public static string GetDllImportPath(string libraryName, bool absolute)
        {
            string nativePath = GetRuntimeNativePath(absolute);
            libraryName = GetNativeLibraryName(libraryName);
            string importPath = Path.Combine(nativePath, libraryName);
            return importPath;
        }
    }
}
