using Abp.CMS;
using Abp.Dependency;
using Abp.Domain.Services;
using Abp.Localization;
using DM.AbpZeroTemplate;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM.AbpZeroDoor.DoorSystem.Enums
{
    /// <summary>
    /// 门禁类型枚举
    /// </summary>
    public enum EDoorType
    {
        /// <summary>
        /// 社区大门
        /// </summary>
        Gate,
        /// <summary>
        /// 邮箱
        /// </summary>
        MailBox,
        /// <summary>
        /// 住户门
        /// </summary>
        Maindoor,
        /// <summary>
        /// 车库
        /// </summary>
        Garage
    }

    /// <summary>
    /// 枚举工具类
    /// </summary>
    public class EDoorTypeUtils
    {
        /// <summary>
        /// 根据枚举类型获取信息
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetValue(EDoorType type)
        {
            switch (type)
            {
                case EDoorType.Gate:
                    return "Gate";
                case EDoorType.MailBox:
                    return "MailBox";
                case EDoorType.Maindoor:
                    return "Maindoor";
                case EDoorType.Garage:
                    return "Garage";
                default:
                    return "FileDoor";
            }
        }

        /// <summary>
        /// 根据枚举类型获取展示信息
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetText(EDoorType type)
        {
            return L(GetValue(type));
        }

        /// <summary>
        ///  根据字符串获取枚举类型
        /// </summary>
        /// <param name="typeStr"></param>
        /// <returns></returns>
        public static EDoorType GetEnum(string typeStr)
        {
            switch (typeStr)
            {
                case "Gate":
                    return EDoorType.Gate;
                case "Garage":
                    return EDoorType.Garage;
                case "MailBox":
                    return EDoorType.MailBox;
                case "Maindoor":
                    return EDoorType.Maindoor;
                default:
                    return EDoorType.Gate;
            }
        }

        /// <summary>
        /// 是否相同
        /// </summary>
        /// <param name="type"></param>
        /// <param name="typeStr"></param>
        /// <returns></returns>
        public static bool Equals(EDoorType type, string typeStr)
        {
            if (String.IsNullOrEmpty(typeStr))
                return false;
            if (GetEnum(typeStr) == type)
                return true;
            return false;
        }

        /// <summary>
        /// 是否相同
        /// </summary>
        /// <param name="type"></param>
        /// <param name="typeStr"></param>
        /// <returns></returns>
        public static bool Equals(string typeStr, EDoorType type)
        {
            return Equals(type, typeStr);
        }

        /// <summary>
        /// 获取HTML字符串
        /// </summary>
        /// <param name="selected"></param>
        /// <returns></returns>
        public static string GetCtrlStr(EDoorType? selected)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("<option value='{0}' {2}>{1}</option>", GetValue(EDoorType.Gate), GetText(EDoorType.Gate), EDoorType.Gate == selected ? "selected='true'" : string.Empty);
            sb.AppendFormat("<option value='{0}' {2}>{1}</option>", GetValue(EDoorType.Maindoor), GetText(EDoorType.Maindoor), EDoorType.Maindoor == selected ? "selected='true'" : string.Empty);
            sb.AppendFormat("<option value='{0}' {2}>{1}</option>", GetValue(EDoorType.Garage), GetText(EDoorType.Garage), EDoorType.Garage == selected ? "selected='true'" : string.Empty);
            sb.AppendFormat("<option value='{0}' {2}>{1}</option>", GetValue(EDoorType.MailBox), GetText(EDoorType.MailBox), EDoorType.MailBox == selected ? "selected='true'" : string.Empty);
            return sb.ToString();
        }

        public static ArrayList GetAllItems(string[] selectedArr)
        {
            ArrayList list = new ArrayList();

            list.Add(new { key = EDoorType.Gate.ToString(), value = L(EDoorType.Gate.ToString()), isSelected = selectedArr.Contains(EDoorType.Gate.ToString()) });

            list.Add(new { key = EDoorType.Maindoor.ToString(), value = L(EDoorType.Maindoor.ToString()), isSelected = selectedArr.Contains(EDoorType.Maindoor.ToString()) });

            list.Add(new { key = EDoorType.Garage.ToString(), value = L(EDoorType.Garage.ToString()), isSelected = selectedArr.Contains(EDoorType.Garage.ToString()) });

            list.Add(new { key = EDoorType.MailBox.ToString(), value = L(EDoorType.MailBox.ToString()), isSelected = selectedArr.Contains(EDoorType.MailBox.ToString()) });

            return list;
        }

        /// <summary>
        /// 本地化
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string L(string name)
        {
            ILocalizationManager localizationManager = IocManager.Instance.Resolve<ILocalizationManager>();
            return localizationManager.GetString(AbpZeroTemplateConsts.LocalizationSourceName, name);
        }
    }
}
