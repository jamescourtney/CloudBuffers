// <auto-generated>
//  automatically generated by the FlatBuffers compiler, do not modify
// </auto-generated>

namespace FlatSharpTests.Oracle
{

using global::System;
using global::FlatBuffers;

public struct OuterStruct : IFlatbufferObject
{
  private Struct __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public void __init(int _i, ByteBuffer _bb) { __p.bb_pos = _i; __p.bb = _bb; }
  public OuterStruct __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  public InnerStruct Inner { get { return (new InnerStruct()).__assign(__p.bb_pos + 0, __p.bb); } }
  public int A { get { return __p.bb.GetInt(__p.bb_pos + 4); } }

  public static Offset<OuterStruct> CreateOuterStruct(FlatBufferBuilder builder, int Inner_A, int A) {
    builder.Prep(4, 8);
    builder.PutInt(A);
    builder.Prep(4, 4);
    builder.PutInt(Inner_A);
    return new Offset<OuterStruct>(builder.Offset);
  }
};


}