using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuffer;
namespace Assets.Scripts.Data.Proto
{
	/// <summary>
	/// </summary>
	public class AllBuffData : ISendable, IReceiveable
	{
		private List<BuffData> buffDataList;
		/// <summary>
		/// 从0级开始
		/// </summary>
		public List<BuffData> BuffDataList
		{
			get
			{
				return buffDataList;
			}
			set
			{
				if(value != null)
				{
					buffDataList = value;
				}
			}
		}

		/// <summary>
		/// </summary>
		public AllBuffData()
		{
			BuffDataList = new List<BuffData>();
		}

		public byte[] GetProtoBufferBytes()
		{
			ProtoBufferWriter writer = new ProtoBufferWriter();
			foreach(BuffData v in BuffDataList)
			{
				writer.Write(1,v);
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
						 var buffData= new BuffData();
						buffData.ParseFrom(obj.Value);
						BuffDataList.Add(buffData);
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
			sb.Append("BuffDataList : [");
			for(int i = 0; i < BuffDataList.Count;i ++)
			{
				if(i == BuffDataList.Count -1)
				{
					sb.Append(BuffDataList[i]);
				}else
				{
					sb.Append(BuffDataList[i] + ",");
				}
			}
			sb.Append("]");
			sb.Append("}");
			return sb.ToString();
		}
	}
}
