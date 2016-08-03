namespace DM.AbpZeroTemplate
{
    public interface IAppFolders
    {
        string TempFileDownloadFolder { get; }

        string SampleProfileImagesFolder { get; }

        string WebLogsFolder { get; set; }

        //上传文件地址配置
        /// <summary>
        /// 普通文件上传路径
        /// </summary>
        string CommonFileFolder { get; set; }
        /// <summary>
        /// 临时文件上传路径
        /// </summary>
        string TempFileFolder { get; set; }
        /// <summary>
        /// 应用普通文件上传路径，使用之前需要替换APP_PATH为真实应用文件夹名称，并创建
        /// </summary>
        string AppCommonFileFolder { get; set; }
        /// <summary>
        /// 应用临时文件上传路径，使用之前需要替换APP_PATH为真实应用文件夹名称，并创建
        /// </summary>
        string AppTempFileFolder { get; set; }

        string AppImageFolder { get; set; }
        string AppVideoFolder { get; set; }
        string AppFileFolder { get; set; }
    }
}