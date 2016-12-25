using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM.AbpZeroTemplate.WebApi.Models
{
    public class GetLunBoContentsInputDto : IInputDto
    {
        /// <summary>
        ///  从第 skipCount+1 条开始
        /// </summary>
        public int SkipCount { get; set; }
        /// <summary>
        ///  每页条数
        /// </summary>
        public int MaxResultCount { get; set; }
        /// <summary>
        ///  小区Id
        /// </summary>
        public long CommunityId { get; set; }

        /// <summary>
        ///  0- IsRecommend  1- IsTop  2- IsColor  3- IsHot
        /// </summary>
        public List<int> SelectTypes { get; set; }

        /// <summary>
        ///  排序字段，可传null
        /// </summary>
        public string Sorting { get; set; }

        /// <summary>
        ///  公司Id
        /// </summary>
        public int? TenantId { get; set; }
        /// <summary>
        /// 栏目Id，可传null
        /// </summary>
        public long? ChannelId { get; set; }
    }
}
