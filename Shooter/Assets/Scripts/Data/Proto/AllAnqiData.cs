using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuffer;
namespace Assets.Scripts.Data.Proto
{
	/// <summary>
	/// </summary>
	public class AllAnqiData : ISendable, IReceiveable
	{
		private List<AnqiData> anqiDataList;
		/// <summary>
		/// 从1级开始
		/// </summary>
		public List<AnqiData> AnqiDataList
		{
			get
			{
				return anqiDataList;
			}
			set
			{
				if(value != null)
				{
					anqiDataList = value;
				}
			}
		}

		/// <summary>
		/// </summary>
		public AllAnqiData()
		{
			AnqiDataList = new List<AnqiData>();
		}

		public byte[] GetProtoBufferBytes()
		{
			ProtoBufferWriter writer = new ProtoBufferWriter();
			foreach(AnqiData v in AnqiDataList)
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
						 var anqiData= new AnqiData();
						anqiData.ParseFrom(obj.Value);
						AnqiDataList.Add(anqiData);
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
			sb.Append("AnqiDataList : [");
			for(int i = 0; i < AnqiDataList.Count;i ++)
			{
				if(i == AnqiDataList.Count -1)
				{
					sb.Append(AnqiDataList[i]);
				}else
				{
					sb.Append(AnqiDataList[i] + ",");
				}
			}
			sb.Append("]");
			sb.Append("}");
			return sb.ToString();
		}
	}
}
