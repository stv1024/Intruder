using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuffer;
namespace Assets.Scripts.Data.Proto
{
	/// <summary>
	/// </summary>
	public class WaveData : ISendable, IReceiveable
	{
		private bool HasBoss{get;set;}
		private bool boss;
		/// <summary>
		/// 是BOSS吗，仅用于判断是否显示WARNING。
		/// </summary>
		public bool Boss
		{
			get
			{
				return boss;
			}
			set
			{
				HasBoss = true;
				boss = value;
			}
		}

		private bool HasEnemyNumberThisTeam{get;set;}
		private int enemyNumberThisTeam;
		/// <summary>
		/// 本波共有多少敌人(仅非BOSS波)
		/// </summary>
		public int EnemyNumberThisTeam
		{
			get
			{
				return enemyNumberThisTeam;
			}
			set
			{
				HasEnemyNumberThisTeam = true;
				enemyNumberThisTeam = value;
			}
		}

		private List<int> enemyTypeThisTeamList;
		/// <summary>
		/// 本波分别有哪几种敌人(仅非BOSS波)
		/// </summary>
		public List<int> EnemyTypeThisTeamList
		{
			get
			{
				return enemyTypeThisTeamList;
			}
			set
			{
				if(value != null)
				{
					enemyTypeThisTeamList = value;
				}
			}
		}

		private List<int> itemTypeThisTeamList;
		/// <summary>
		/// 本波会掉哪些道具
		/// </summary>
		public List<int> ItemTypeThisTeamList
		{
			get
			{
				return itemTypeThisTeamList;
			}
			set
			{
				if(value != null)
				{
					itemTypeThisTeamList = value;
				}
			}
		}

		private List<float> itemOddsThisTeamList;
		/// <summary>
		/// 本波敌人所掉道具的掉率
		/// </summary>
		public List<float> ItemOddsThisTeamList
		{
			get
			{
				return itemOddsThisTeamList;
			}
			set
			{
				if(value != null)
				{
					itemOddsThisTeamList = value;
				}
			}
		}

		private bool HasEnemyTeamColdTime{get;set;}
		private float enemyTeamColdTime;
		/// <summary>
		/// 与上一波之间间隔多少秒
		/// </summary>
		public float EnemyTeamColdTime
		{
			get
			{
				return enemyTeamColdTime;
			}
			set
			{
				HasEnemyTeamColdTime = true;
				enemyTeamColdTime = value;
			}
		}

		private List<int> bossEnemyTypeList;
		/// <summary>
		/// 哪几种BOSS(仅BOSS波)
		/// </summary>
		public List<int> BossEnemyTypeList
		{
			get
			{
				return bossEnemyTypeList;
			}
			set
			{
				if(value != null)
				{
					bossEnemyTypeList = value;
				}
			}
		}

		private List<int> bossEnemyCountList;
		/// <summary>
		/// 每种BOSS出场几个(仅BOSS波)
		/// </summary>
		public List<int> BossEnemyCountList
		{
			get
			{
				return bossEnemyCountList;
			}
			set
			{
				if(value != null)
				{
					bossEnemyCountList = value;
				}
			}
		}

		private bool HasShowBossCome{get;set;}
		private bool showBossCome;
		/// <summary>
		/// 是否有魔王降临效果
		/// </summary>
		public bool ShowBossCome
		{
			get
			{
				return showBossCome;
			}
			set
			{
				HasShowBossCome = true;
				showBossCome = value;
			}
		}

		/// <summary>
		/// </summary>
		public WaveData()
		{
			EnemyTypeThisTeamList = new List<int>();
			ItemTypeThisTeamList = new List<int>();
			ItemOddsThisTeamList = new List<float>();
			BossEnemyTypeList = new List<int>();
			BossEnemyCountList = new List<int>();
		}

		/// <summary>
		/// </summary>
		public WaveData
		(
			bool boss,
			int enemyNumberThisTeam,
			float enemyTeamColdTime,
			bool showBossCome
		):this()
		{
			Boss = boss;
			EnemyNumberThisTeam = enemyNumberThisTeam;
			EnemyTeamColdTime = enemyTeamColdTime;
			ShowBossCome = showBossCome;
		}
		public byte[] GetProtoBufferBytes()
		{
			ProtoBufferWriter writer = new ProtoBufferWriter();
			writer.Write(1,Boss);
			writer.Write(2,EnemyNumberThisTeam);
			foreach(int v in EnemyTypeThisTeamList)
			{
				writer.Write(3,v);
			}
			foreach(int v in ItemTypeThisTeamList)
			{
				writer.Write(4,v);
			}
			foreach(float v in ItemOddsThisTeamList)
			{
				writer.Write(5,v);
			}
			writer.Write(6,EnemyTeamColdTime);
			foreach(int v in BossEnemyTypeList)
			{
				writer.Write(11,v);
			}
			foreach(int v in BossEnemyCountList)
			{
				writer.Write(12,v);
			}
			writer.Write(13,ShowBossCome);
			return writer.GetProtoBufferBytes();
		}
		public void ParseFrom(byte[] buffer)
		{
			 ParseFrom(buffer, 0, buffer.Length);
		}
		public void ParseFrom(byte[] buffer, int offset, int size)
		{
			if (buffer == null) return;
			ProtoBufferReader reader = new ProtoBufferReader(buffer,offset,size);
			foreach (ProtoBufferObject obj in reader.ProtoBufferObjs)
			{
				switch (obj.FieldNumber)
				{
					case 1:
						Boss = obj.Value;
						break;
					case 2:
						EnemyNumberThisTeam = obj.Value;
						break;
					case 3:
						EnemyTypeThisTeamList.Add(obj.Value);
						break;
					case 4:
						ItemTypeThisTeamList.Add(obj.Value);
						break;
					case 5:
						ItemOddsThisTeamList.Add(obj.Value);
						break;
					case 6:
						EnemyTeamColdTime = obj.Value;
						break;
					case 11:
						BossEnemyTypeList.Add(obj.Value);
						break;
					case 12:
						BossEnemyCountList.Add(obj.Value);
						break;
					case 13:
						ShowBossCome = obj.Value;
						break;
					default:
						break;
				}
			}
		}
		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("{");
			sb.Append("Boss : " + Boss + ",");
			sb.Append("EnemyNumberThisTeam : " + EnemyNumberThisTeam + ",");
			sb.Append("EnemyTypeThisTeamList : [");
			for(int i = 0; i < EnemyTypeThisTeamList.Count;i ++)
			{
				if(i == EnemyTypeThisTeamList.Count -1)
				{
					sb.Append(EnemyTypeThisTeamList[i]);
				}else
				{
					sb.Append(EnemyTypeThisTeamList[i] + ",");
				}
			}
			sb.Append("],");
			sb.Append("ItemTypeThisTeamList : [");
			for(int i = 0; i < ItemTypeThisTeamList.Count;i ++)
			{
				if(i == ItemTypeThisTeamList.Count -1)
				{
					sb.Append(ItemTypeThisTeamList[i]);
				}else
				{
					sb.Append(ItemTypeThisTeamList[i] + ",");
				}
			}
			sb.Append("],");
			sb.Append("ItemOddsThisTeamList : [");
			for(int i = 0; i < ItemOddsThisTeamList.Count;i ++)
			{
				if(i == ItemOddsThisTeamList.Count -1)
				{
					sb.Append(ItemOddsThisTeamList[i]);
				}else
				{
					sb.Append(ItemOddsThisTeamList[i] + ",");
				}
			}
			sb.Append("],");
			sb.Append("EnemyTeamColdTime : " + EnemyTeamColdTime + ",");
			sb.Append("BossEnemyTypeList : [");
			for(int i = 0; i < BossEnemyTypeList.Count;i ++)
			{
				if(i == BossEnemyTypeList.Count -1)
				{
					sb.Append(BossEnemyTypeList[i]);
				}else
				{
					sb.Append(BossEnemyTypeList[i] + ",");
				}
			}
			sb.Append("],");
			sb.Append("BossEnemyCountList : [");
			for(int i = 0; i < BossEnemyCountList.Count;i ++)
			{
				if(i == BossEnemyCountList.Count -1)
				{
					sb.Append(BossEnemyCountList[i]);
				}else
				{
					sb.Append(BossEnemyCountList[i] + ",");
				}
			}
			sb.Append("],");
			sb.Append("ShowBossCome : " + ShowBossCome);
			sb.Append("}");
			return sb.ToString();
		}
	}
}
