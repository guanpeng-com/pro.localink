using Abp.Dependency;

namespace DM.AbpZeroTemplate
{
    public class AppFolders : IAppFolders, ISingletonDependency
    {
        public string TempFileDownloadFolder { get; set; }

        public string SampleProfileImagesFolder { get; set; }

        public string WebLogsFolder { get; set; }


        //上传文件地址配置
        /// <summary>
        /// 普通文件上传路径
        /// </summary>
        public string CommonFileFolder { get; set; }
        /// <summary>
        /// 临时文件上传路径
        /// </summary>
        public string TempFileFolder { get; set; }
        /// <summary>
        /// 应用普通文件上传路径，使用之前需要替换APP_PATH为真实应用文件夹名称，并创建
        /// </summary>
        public string AppCommonFileFolder { get; set; }
        /// <summary>
        /// 应用临时文件上传路径，使用之前需要替换APP_PATH为真实应用文件夹名称，并创建
        /// </summary>
        public string AppTempFileFolder { get; set; }

        public string AppImageFolder { get; set; }
        public string AppVideoFolder { get; set; }
        public string AppFileFolder { get; set; }
    }
}