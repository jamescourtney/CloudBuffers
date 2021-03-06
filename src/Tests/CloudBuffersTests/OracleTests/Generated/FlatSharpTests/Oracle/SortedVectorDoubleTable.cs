// <auto-generated>
//  automatically generated by the FlatBuffers compiler, do not modify
// </auto-generated>

namespace FlatSharpTests.Oracle
{

using global::System;
using global::System.Collections.Generic;
using global::FlatBuffers;

public struct SortedVectorDoubleTable : IFlatbufferObject
{
  private Table __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public static void ValidateVersion() { FlatBufferConstants.FLATBUFFERS_2_0_0(); }
  public static SortedVectorDoubleTable GetRootAsSortedVectorDoubleTable(ByteBuffer _bb) { return GetRootAsSortedVectorDoubleTable(_bb, new SortedVectorDoubleTable()); }
  public static SortedVectorDoubleTable GetRootAsSortedVectorDoubleTable(ByteBuffer _bb, SortedVectorDoubleTable obj) { return (obj.__assign(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public void __init(int _i, ByteBuffer _bb) { __p = new Table(_i, _bb); }
  public SortedVectorDoubleTable __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  public double Value { get { int o = __p.__offset(4); return o != 0 ? __p.bb.GetDouble(o + __p.bb_pos) : (double)0.0; } }

  public static Offset<FlatSharpTests.Oracle.SortedVectorDoubleTable> CreateSortedVectorDoubleTable(FlatBufferBuilder builder,
      double Value = 0.0) {
    builder.StartTable(1);
    SortedVectorDoubleTable.AddValue(builder, Value);
    return SortedVectorDoubleTable.EndSortedVectorDoubleTable(builder);
  }

  public static void StartSortedVectorDoubleTable(FlatBufferBuilder builder) { builder.StartTable(1); }
  public static void AddValue(FlatBufferBuilder builder, double Value) { builder.AddDouble(0, Value, 0.0); }
  public static Offset<FlatSharpTests.Oracle.SortedVectorDoubleTable> EndSortedVectorDoubleTable(FlatBufferBuilder builder) {
    int o = builder.EndTable();
    return new Offset<FlatSharpTests.Oracle.SortedVectorDoubleTable>(o);
  }

  public static VectorOffset CreateSortedVectorOfSortedVectorDoubleTable(FlatBufferBuilder builder, Offset<SortedVectorDoubleTable>[] offsets) {
    Array.Sort(offsets, (Offset<SortedVectorDoubleTable> o1, Offset<SortedVectorDoubleTable> o2) => builder.DataBuffer.GetDouble(Table.__offset(4, o1.Value, builder.DataBuffer)).CompareTo(builder.DataBuffer.GetDouble(Table.__offset(4, o2.Value, builder.DataBuffer))));
    return builder.CreateVectorOfTables(offsets);
  }

  public static SortedVectorDoubleTable? __lookup_by_key(int vectorLocation, double key, ByteBuffer bb) {
    int span = bb.GetInt(vectorLocation - 4);
    int start = 0;
    while (span != 0) {
      int middle = span / 2;
      int tableOffset = Table.__indirect(vectorLocation + 4 * (start + middle), bb);
      int comp = bb.GetDouble(Table.__offset(4, bb.Length - tableOffset, bb)).CompareTo(key);
      if (comp > 0) {
        span = middle;
      } else if (comp < 0) {
        middle++;
        start += middle;
        span -= middle;
      } else {
        return new SortedVectorDoubleTable().__assign(tableOffset, bb);
      }
    }
    return null;
  }
  public SortedVectorDoubleTableT UnPack() {
    var _o = new SortedVectorDoubleTableT();
    this.UnPackTo(_o);
    return _o;
  }
  public void UnPackTo(SortedVectorDoubleTableT _o) {
    _o.Value = this.Value;
  }
  public static Offset<FlatSharpTests.Oracle.SortedVectorDoubleTable> Pack(FlatBufferBuilder builder, SortedVectorDoubleTableT _o) {
    if (_o == null) return default(Offset<FlatSharpTests.Oracle.SortedVectorDoubleTable>);
    return CreateSortedVectorDoubleTable(
      builder,
      _o.Value);
  }
}

public class SortedVectorDoubleTableT
{
  public double Value { get; set; }

  public SortedVectorDoubleTableT() {
    this.Value = 0.0;
  }
}


}
