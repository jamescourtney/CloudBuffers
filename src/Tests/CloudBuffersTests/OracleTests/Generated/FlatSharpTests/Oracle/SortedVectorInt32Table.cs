// <auto-generated>
//  automatically generated by the FlatBuffers compiler, do not modify
// </auto-generated>

namespace FlatSharpTests.Oracle
{

using global::System;
using global::System.Collections.Generic;
using global::FlatBuffers;

public struct SortedVectorInt32Table : IFlatbufferObject
{
  private Table __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public static void ValidateVersion() { FlatBufferConstants.FLATBUFFERS_2_0_0(); }
  public static SortedVectorInt32Table GetRootAsSortedVectorInt32Table(ByteBuffer _bb) { return GetRootAsSortedVectorInt32Table(_bb, new SortedVectorInt32Table()); }
  public static SortedVectorInt32Table GetRootAsSortedVectorInt32Table(ByteBuffer _bb, SortedVectorInt32Table obj) { return (obj.__assign(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public void __init(int _i, ByteBuffer _bb) { __p = new Table(_i, _bb); }
  public SortedVectorInt32Table __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  public int Value { get { int o = __p.__offset(4); return o != 0 ? __p.bb.GetInt(o + __p.bb_pos) : (int)5; } }

  public static Offset<FlatSharpTests.Oracle.SortedVectorInt32Table> CreateSortedVectorInt32Table(FlatBufferBuilder builder,
      int Value = 5) {
    builder.StartTable(1);
    SortedVectorInt32Table.AddValue(builder, Value);
    return SortedVectorInt32Table.EndSortedVectorInt32Table(builder);
  }

  public static void StartSortedVectorInt32Table(FlatBufferBuilder builder) { builder.StartTable(1); }
  public static void AddValue(FlatBufferBuilder builder, int Value) { builder.AddInt(0, Value, 5); }
  public static Offset<FlatSharpTests.Oracle.SortedVectorInt32Table> EndSortedVectorInt32Table(FlatBufferBuilder builder) {
    int o = builder.EndTable();
    return new Offset<FlatSharpTests.Oracle.SortedVectorInt32Table>(o);
  }

  public static VectorOffset CreateSortedVectorOfSortedVectorInt32Table(FlatBufferBuilder builder, Offset<SortedVectorInt32Table>[] offsets) {
    Array.Sort(offsets, (Offset<SortedVectorInt32Table> o1, Offset<SortedVectorInt32Table> o2) => builder.DataBuffer.GetInt(Table.__offset(4, o1.Value, builder.DataBuffer)).CompareTo(builder.DataBuffer.GetInt(Table.__offset(4, o2.Value, builder.DataBuffer))));
    return builder.CreateVectorOfTables(offsets);
  }

  public static SortedVectorInt32Table? __lookup_by_key(int vectorLocation, int key, ByteBuffer bb) {
    int span = bb.GetInt(vectorLocation - 4);
    int start = 0;
    while (span != 0) {
      int middle = span / 2;
      int tableOffset = Table.__indirect(vectorLocation + 4 * (start + middle), bb);
      int comp = bb.GetInt(Table.__offset(4, bb.Length - tableOffset, bb)).CompareTo(key);
      if (comp > 0) {
        span = middle;
      } else if (comp < 0) {
        middle++;
        start += middle;
        span -= middle;
      } else {
        return new SortedVectorInt32Table().__assign(tableOffset, bb);
      }
    }
    return null;
  }
  public SortedVectorInt32TableT UnPack() {
    var _o = new SortedVectorInt32TableT();
    this.UnPackTo(_o);
    return _o;
  }
  public void UnPackTo(SortedVectorInt32TableT _o) {
    _o.Value = this.Value;
  }
  public static Offset<FlatSharpTests.Oracle.SortedVectorInt32Table> Pack(FlatBufferBuilder builder, SortedVectorInt32TableT _o) {
    if (_o == null) return default(Offset<FlatSharpTests.Oracle.SortedVectorInt32Table>);
    return CreateSortedVectorInt32Table(
      builder,
      _o.Value);
  }
}

public class SortedVectorInt32TableT
{
  public int Value { get; set; }

  public SortedVectorInt32TableT() {
    this.Value = 5;
  }
}


}
