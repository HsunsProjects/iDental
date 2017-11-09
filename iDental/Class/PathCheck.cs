using System.IO;

namespace iDental.Class
{
    public static class PathCheck
    {
        /// <summary>
        /// 檢查路徑是否存在
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool IsPathExist(string path)
        {
            if (Directory.Exists(path))
                return true;
            else
                return false;
        }

        /// <summary>
        /// 檢查路徑，如果不存在就新增
        /// </summary>
        /// <param name="path"></param>
        public static void CheckPathAndCreate(string path)
        {
            if (!IsPathExist(path))
                Directory.CreateDirectory(path);
        }

        /// <summary>
        /// 檢查檔案是否存在
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static bool IsFileExist(string fileName)
        {
            if (File.Exists(fileName))
                return true;
            else
                return false;
        }
    }
}
