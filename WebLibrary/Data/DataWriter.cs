using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WebLibrary.Data {
    public static class DataWriter {
        public const string IMG_PATH = "wwwroot/files/logos/";
        public const string VIDEO_PATH = "wwwroot/files/videos/";

        private const string IMG_EXTENTIONS = ".jpeg .jpg .png";
        private const string VIDEO_EXTENTIONS = ".mp4 .mpeg .mpg .avi";

        public static string AddImage(IFormFile image) {
            if (IsImage(image.FileName)) {
                return AddFile(IMG_PATH, image);
            }
            return null;
        }

        public static string AddVideo(IFormFile video) {
            if (IsVideo(video.FileName)) {
                return AddFile(VIDEO_PATH, video);
            }
            return null;
        }

        public static void RemoveImage(string name) {
            if (name != null) {
                RemoveFile(IMG_PATH, name);
            }
        }

        public static void RemoveVideo(string name) {
            if (name != null) 
                RemoveFile(VIDEO_PATH, name);
        }

        private static void RemoveFile(string dir, string name) {
            string path = dir + name;
            if (File.Exists(path))
                File.Delete(path);
        }

        private static string AddFile(string dir, IFormFile file) {
            string fileName;
            try {
                fileName = GetAppropriateName(dir, file.FileName);
                using (var fileStream = new FileStream(dir + fileName, FileMode.Create)) {
                    file.CopyTo(fileStream);
                }
            }
            catch (IOException) {
                return null;
            }

            return fileName;
        }

        private static string GetAppropriateName(string dir, string fname) {
            string ext = Path.GetExtension(fname);
            string name = Path.GetFileNameWithoutExtension(fname);

            string path = dir + fname;
            int i = 0;
            while (File.Exists(path)) {
                i++;
                path = $"{dir}{name}({i}).{ext}";
            }

            return i == 0 ? fname : $"{name}({i}).{ext}";
        }

        private static bool IsImage(string fname) {
            return IMG_EXTENTIONS.IndexOf(Path.GetExtension(fname)) != -1;
        }

        private static bool IsVideo(string fname) {
            return VIDEO_EXTENTIONS.IndexOf(Path.GetExtension(fname)) != -1;
        }
    }
}
