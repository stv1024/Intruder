using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuffer;
namespace Assets.Scripts.Data.Proto
{
	/// <summary>
	/// </summary>
	public class BuffData : ISendable, IReceiveable
	{
		private List<float> durationList;
		/// <summary>
		/// 各级持续时间，标识该Buff共有多少级
		/// </summary>
		public List<float> DurationList
		{
			get
			{
				return durationList;
			}
			set
			{
				if(value != null)
				{
					durationList = value;
				}
			}
		}

		private List<float> num0List;
		/// <summary>
		/// 等级相关数值0。回天:回复血量;雷障:吸收伤害
		/// </summary>
		public List<float> Num0List
		{
			get
			{
				return num0List;
			}
			set
			{
				if(value != null)
				{
					num0List = value;
				}
			}
		}

		/// <summary>
		/// </summary>
		public BuffData()
		{
			DurationList = new List<float>();
			Num0List = new List<float>();
		}

		public byte[] GetProtoBufferBytes()
		{
			ProtoBufferWriter writer = new ProtoBufferWriter();
			foreach(float v in DurationList)
			{
				writer.Write(1,v);
			}
			foreach(float v in Num0List)
			{
				writer.Write(2,v);
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
						DurationList.Add(obj.Value);
						break;
					case 2:
						Num0List.Add(obj.Value);
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
			sb.Append("DurationList : [");
			for(int i = 0; i < DurationList.Count;i ++)
			{
				if(i == DurationList.Count -1)
				{
					sb.Append(DurationList[i]);
				}else
				{
					sb.Append(DurationList[i] + ",");
				}
			}
			sb.Append("],");
			sb.Append("Num0List : [");
			for(int i = 0; i < Num0List.Count;i ++)
			{
				if(i == Num0List.Count -1)
				{
					sb.Append(Num0List[i]);
				}else
				{
					sb.Append(Num0List[i] + ",");
				}
			}
			sb.Append("]");
			sb.Append("}");
			return sb.ToString();
		}
	}
}
