using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuffer;
namespace Assets.Scripts.Data.Proto
{
	/// <summary>
	/// </summary>
	public class AnqiData : ISendable, IReceiveable
	{
		private List<float> nonLevelNumList;
		/// <summary>
		/// 与等级无关的数值。苦无:攻击力、飞行速度;球形闪电:攻击力、间隔
		/// </summary>
		public List<float> NonLevelNumList
		{
			get
			{
				return nonLevelNumList;
			}
			set
			{
				if(value != null)
				{
					nonLevelNumList = value;
				}
			}
		}

		private bool HasLevelCount{get;set;}
		private int levelCount;
		/// <summary>
		/// 唯一标识有多少级的。
		/// </summary>
		public int LevelCount
		{
			get
			{
				return levelCount;
			}
			set
			{
				HasLevelCount = true;
				levelCount = value;
			}
		}

		private List<float> levelNum0List;
		/// <summary>
		/// 等级相关数值0，从1级开始。苦无:间隔;球形闪电:范围
		/// </summary>
		public List<float> LevelNum0List
		{
			get
			{
				return levelNum0List;
			}
			set
			{
				if(value != null)
				{
					levelNum0List = value;
				}
			}
		}

		/// <summary>
		/// </summary>
		public AnqiData()
		{
			NonLevelNumList = new List<float>();
			LevelNum0List = new List<float>();
		}

		/// <summary>
		/// </summary>
		public AnqiData
		(
			int levelCount
		):this()
		{
			LevelCount = levelCount;
		}
		public byte[] GetProtoBufferBytes()
		{
			ProtoBufferWriter writer = new ProtoBufferWriter();
			foreach(float v in NonLevelNumList)
			{
				writer.Write(1,v);
			}
			writer.Write(2,LevelCount);
			foreach(float v in LevelNum0List)
			{
				writer.Write(3,v);
			}
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
						NonLevelNumList.Add(obj.Value);
						break;
					case 2:
						LevelCount = obj.Value;
						break;
					case 3:
						LevelNum0List.Add(obj.Value);
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
			sb.Append("NonLevelNumList : [");
			for(int i = 0; i < NonLevelNumList.Count;i ++)
			{
				if(i == NonLevelNumList.Count -1)
				{
					sb.Append(NonLevelNumList[i]);
				}else
				{
					sb.Append(NonLevelNumList[i] + ",");
				}
			}
			sb.Append("],");
			sb.Append("LevelCount : " + LevelCount + ",");
			sb.Append("LevelNum0List : [");
			for(int i = 0; i < LevelNum0List.Count;i ++)
			{
				if(i == LevelNum0List.Count -1)
				{
					sb.Append(LevelNum0List[i]);
				}else
				{
					sb.Append(LevelNum0List[i] + ",");
				}
			}
			sb.Append("]");
			sb.Append("}");
			return sb.ToString();
		}
	}
}
