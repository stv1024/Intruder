using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuffer;
namespace Assets.Scripts.Data.Proto
{
	/// <summary>
	/// 所有关卡数据
	/// </summary>
	public class AllLevelData : ISendable, IReceiveable
	{
		private List<LevelData> levelDataList;
		/// <summary>
		/// </summary>
		public List<LevelData> LevelDataList
		{
			get
			{
				return levelDataList;
			}
			set
			{
				if(value != null)
				{
					levelDataList = value;
				}
			}
		}

		/// <summary>
		/// 所有关卡数据
		/// </summary>
		public AllLevelData()
		{
			LevelDataList = new List<LevelData>();
		}

		public byte[] GetProtoBufferBytes()
		{
			ProtoBufferWriter writer = new ProtoBufferWriter();
			foreach(LevelData v in LevelDataList)
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
						 var levelData= new LevelData();
						levelData.ParseFrom(obj.Value);
						LevelDataList.Add(levelData);
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
			sb.Append("LevelDataList : [");
			for(int i = 0; i < LevelDataList.Count;i ++)
			{
				if(i == LevelDataList.Count -1)
				{
					sb.Append(LevelDataList[i]);
				}else
				{
					sb.Append(LevelDataList[i] + ",");
				}
			}
			sb.Append("]");
			sb.Append("}");
			return sb.ToString();
		}
	}
}
