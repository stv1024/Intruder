using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuffer;
namespace Assets.Scripts.Data.Proto
{
	/// <summary>
	/// </summary>
	public class ForeverData : ISendable, IReceiveable
	{
		private bool HasCoin{get;set;}
		private int coin;
		/// <summary>
		/// </summary>
		public int Coin
		{
			get
			{
				return coin;
			}
			set
			{
				HasCoin = true;
				coin = value;
			}
		}

		private List<int> buffLevelList;
		/// <summary>
		/// 各Buff等级。等级从0开始。
		/// </summary>
		public List<int> BuffLevelList
		{
			get
			{
				return buffLevelList;
			}
			set
			{
				if(value != null)
				{
					buffLevelList = value;
				}
			}
		}

		private List<int> anqiLevelList;
		/// <summary>
		/// 各暗器等级。等级从1开始。
		/// </summary>
		public List<int> AnqiLevelList
		{
			get
			{
				return anqiLevelList;
			}
			set
			{
				if(value != null)
				{
					anqiLevelList = value;
				}
			}
		}

		private List<bool> changshengwanEatenList;
		/// <summary>
		/// 编号的长生丸，哪些吃过了。也用于计算主角初始生命值。
		/// </summary>
		public List<bool> ChangshengwanEatenList
		{
			get
			{
				return changshengwanEatenList;
			}
			set
			{
				if(value != null)
				{
					changshengwanEatenList = value;
				}
			}
		}

		private bool HasBishaLevel{get;set;}
		private int bishaLevel;
		/// <summary>
		/// 必杀等级，从0开始。
		/// </summary>
		public int BishaLevel
		{
			get
			{
				return bishaLevel;
			}
			set
			{
				HasBishaLevel = true;
				bishaLevel = value;
			}
		}

		private bool HasHighestScore{get;set;}
		private int highestScore;
		/// <summary>
		/// 该设备最高分
		/// </summary>
		public int HighestScore
		{
			get
			{
				return highestScore;
			}
			set
			{
				HasHighestScore = true;
				highestScore = value;
			}
		}

		private bool HasNickname{get;set;}
		private string nickname;
		/// <summary>
		/// 玩家输入的昵称
		/// </summary>
		public string Nickname
		{
			get
			{
				return nickname;
			}
			set
			{
				if(value != null)
				{
					HasNickname = true;
					nickname = value;
				}
			}
		}

		/// <summary>
		/// </summary>
		public ForeverData()
		{
			BuffLevelList = new List<int>();
			AnqiLevelList = new List<int>();
			ChangshengwanEatenList = new List<bool>();
		}

		/// <summary>
		/// </summary>
		public ForeverData
		(
			int coin,
			int bishaLevel,
			int highestScore,
			string nickname
		):this()
		{
			Coin = coin;
			BishaLevel = bishaLevel;
			HighestScore = highestScore;
			Nickname = nickname;
		}
		public byte[] GetProtoBufferBytes()
		{
			ProtoBufferWriter writer = new ProtoBufferWriter();
			writer.Write(1,Coin);
			foreach(int v in BuffLevelList)
			{
				writer.Write(2,v);
			}
			foreach(int v in AnqiLevelList)
			{
				writer.Write(3,v);
			}
			foreach(bool v in ChangshengwanEatenList)
			{
				writer.Write(4,v);
			}
			writer.Write(5,BishaLevel);
			writer.Write(6,HighestScore);
			writer.Write(7,Nickname);
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
						Coin = obj.Value;
						break;
					case 2:
						BuffLevelList.Add(obj.Value);
						break;
					case 3:
						AnqiLevelList.Add(obj.Value);
						break;
					case 4:
						ChangshengwanEatenList.Add(obj.Value);
						break;
					case 5:
						BishaLevel = obj.Value;
						break;
					case 6:
						HighestScore = obj.Value;
						break;
					case 7:
						Nickname = obj.Value;
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
			sb.Append("Coin : " + Coin + ",");
			sb.Append("BuffLevelList : [");
			for(int i = 0; i < BuffLevelList.Count;i ++)
			{
				if(i == BuffLevelList.Count -1)
				{
					sb.Append(BuffLevelList[i]);
				}else
				{
					sb.Append(BuffLevelList[i] + ",");
				}
			}
			sb.Append("],");
			sb.Append("AnqiLevelList : [");
			for(int i = 0; i < AnqiLevelList.Count;i ++)
			{
				if(i == AnqiLevelList.Count -1)
				{
					sb.Append(AnqiLevelList[i]);
				}else
				{
					sb.Append(AnqiLevelList[i] + ",");
				}
			}
			sb.Append("],");
			sb.Append("ChangshengwanEatenList : [");
			for(int i = 0; i < ChangshengwanEatenList.Count;i ++)
			{
				if(i == ChangshengwanEatenList.Count -1)
				{
					sb.Append(ChangshengwanEatenList[i]);
				}else
				{
					sb.Append(ChangshengwanEatenList[i] + ",");
				}
			}
			sb.Append("],");
			sb.Append("BishaLevel : " + BishaLevel + ",");
			sb.Append("HighestScore : " + HighestScore + ",");
			sb.Append("Nickname : " + Nickname);
			sb.Append("}");
			return sb.ToString();
		}
	}
}
