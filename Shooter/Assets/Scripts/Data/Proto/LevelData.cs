using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuffer;
namespace Assets.Scripts.Data.Proto
{
	/// <summary>
	/// </summary>
	public class LevelData : ISendable, IReceiveable
	{
		private List<WaveData> waveDataList;
		/// <summary>
		/// 本关有哪几波
		/// </summary>
		public List<WaveData> WaveDataList
		{
			get
			{
				return waveDataList;
			}
			set
			{
				if(value != null)
				{
					waveDataList = value;
				}
			}
		}

		/// <summary>
		/// </summary>
		public LevelData()
		{
			WaveDataList = new List<WaveData>();
		}

		public byte[] GetProtoBufferBytes()
		{
			ProtoBufferWriter writer = new ProtoBufferWriter();
			foreach(WaveData v in WaveDataList)
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
						 var waveData= new WaveData();
						waveData.ParseFrom(obj.Value);
						WaveDataList.Add(waveData);
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
			sb.Append("WaveDataList : [");
			for(int i = 0; i < WaveDataList.Count;i ++)
			{
				if(i == WaveDataList.Count -1)
				{
					sb.Append(WaveDataList[i]);
				}else
				{
					sb.Append(WaveDataList[i] + ",");
				}
			}
			sb.Append("]");
			sb.Append("}");
			return sb.ToString();
		}
	}
}
