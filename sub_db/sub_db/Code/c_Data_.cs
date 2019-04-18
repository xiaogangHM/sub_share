﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Data;

namespace sub_db
{
	internal class c_Data_
	{
		// 字幕信息
		internal class c_SubInfo
		{
			// 番剧名称
			internal string		m_name_chs	= "";					// (简体)
			internal string		m_name_cht	= "";					// (繁体)
			internal string		m_name_jp	= "";					// (日文)
			internal string		m_name_en	= "";					// (英文)
			internal string		m_name_rome	= "";					// (罗马字)

			internal DateTime	m_time		= DateTime.MinValue;	// 最早放送日期
			internal string		m_type		= "";					// 分类（animation/movie/...）
			internal string		m_source	= "";					// 字幕匹配的片源（DB/DVD/TV/test/...）
			internal string		m_sub_name	= "";					// 字幕名称
			internal string		m_extension	= "";					// 字幕后缀名（多种格式时用;分割，例如：.ass;.srt）
			internal string		m_providers	= "";					// 字幕组/提供者
			internal string		m_desc		= "";					// 字幕说明
		};

		// 字幕列表（按文件顺序读取）
		internal static List<c_SubInfo>			m_s_all_subs	= new List<c_SubInfo>();
		internal static ReaderWriterLockSlim	m_s_lock		= new ReaderWriterLockSlim();

		internal static DataTable				m_s_dt			= new DataTable("subs");
		internal static DataTable				m_s_dt_search	= new DataTable("subs");

		// 数据各列的名字
		internal enum e_ColumnName
		{
			name_chs,
			name_cht,
			name_jp,
			name_en,
			name_rome,

			time,
			type,
			source,
			sub_name,
			extension,
			providers,
			desc,

			MAX,
		};

		/*==============================================================
		 * 更新 DataTable
		 *==============================================================*/
		internal static void	update_DataTable()
		{
			if(m_s_dt.Columns.Count == 0)
			{
				string[] columns_desc =
				{
					e_ColumnName.name_chs.ToString(),	"System.String",
					e_ColumnName.name_cht.ToString(),	"System.String",
					e_ColumnName.name_jp.ToString(),	"System.String",
					e_ColumnName.name_en.ToString(),	"System.String",
					e_ColumnName.name_rome.ToString(),	"System.String",

					e_ColumnName.time.ToString(),		"System.DateTime",
					e_ColumnName.type.ToString(),		"System.String",
					e_ColumnName.source.ToString(),		"System.String",
					e_ColumnName.sub_name.ToString(),	"System.String",
					e_ColumnName.extension.ToString(),	"System.String",
					e_ColumnName.providers.ToString(),	"System.String",
					e_ColumnName.desc.ToString(),		"System.String",
				};

				for(int i=0; i<columns_desc.Length; i+=2)
				{
					m_s_dt.Columns.Add(columns_desc[i], Type.GetType(columns_desc[i + 1]));
					m_s_dt_search.Columns.Add(columns_desc[i], Type.GetType(columns_desc[i + 1]));
				}
			}

			m_s_lock.EnterWriteLock();

			m_s_dt.Rows.Clear();

			foreach(c_SubInfo sub_info in m_s_all_subs)
			{
				DataRow dr = m_s_dt.NewRow();
				dr[e_ColumnName.name_chs.ToString()]	= sub_info.m_name_chs;
				dr[e_ColumnName.name_cht.ToString()]	= sub_info.m_name_cht;
				dr[e_ColumnName.name_jp.ToString()]		= sub_info.m_name_jp;
				dr[e_ColumnName.name_en.ToString()]		= sub_info.m_name_en;
				dr[e_ColumnName.name_rome.ToString()]	= sub_info.m_name_rome;

				dr[e_ColumnName.time.ToString()]		= sub_info.m_time;
				dr[e_ColumnName.type.ToString()]		= sub_info.m_type;
				dr[e_ColumnName.source.ToString()]		= sub_info.m_source;
				dr[e_ColumnName.sub_name.ToString()]	= sub_info.m_sub_name;
				dr[e_ColumnName.extension.ToString()]	= sub_info.m_extension;
				dr[e_ColumnName.providers.ToString()]	= sub_info.m_providers;
				dr[e_ColumnName.desc.ToString()]		= sub_info.m_desc;

				m_s_dt.Rows.Add(dr);
			}	// for

			m_s_lock.ExitWriteLock();
		}
	};
}	// namespace sub_db