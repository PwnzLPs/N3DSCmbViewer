﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace N3DSCmbViewer.Cmb
{
    [System.Diagnostics.DebuggerDisplay("{GetType()}")]
    class SepdChunk : BaseCTRChunk
    {
        // ????
        public override string ChunkTag { get { return "sepd"; } }

        // ??
        public ushort PrmsCount { get; private set; }
        public ushort Unknown0A { get; private set; }
        public float UnknownFloat0C { get; private set; }
        public float UnknownFloat10 { get; private set; }
        public float UnknownFloat14 { get; private set; }
        public uint Unknown18 { get; private set; }
        public uint Unknown1C { get; private set; }
        public uint Unknown20 { get; private set; }

        public uint[] ArrayOffsets { get; private set; }
        public float[] ArrayScales { get; private set; }
        public Constants.DataTypes[] ArrayDataTypes { get; private set; }
        public ushort[] ArrayUnknown1 { get; private set; }
        public uint[] ArrayUnknown2 { get; private set; }
        public uint[] ArrayUnknown3 { get; private set; }
        public uint[] ArrayUnknown4 { get; private set; }
        public uint[] ArrayUnknown5 { get; private set; }

        public uint VertexArrayOffset { get { return ArrayOffsets[BaseCTRChunk.IsMajora3D ? VatrChunk.VertexArray_MM : VatrChunk.VertexArray_OoT]; } }
        public float VertexArrayScale { get { return ArrayScales[BaseCTRChunk.IsMajora3D ? VatrChunk.VertexArray_MM : VatrChunk.VertexArray_OoT]; } }
        public Constants.DataTypes VertexArrayDataType { get { return ArrayDataTypes[BaseCTRChunk.IsMajora3D ? VatrChunk.VertexArray_MM : VatrChunk.VertexArray_OoT]; } }

        public uint NormalArrayOffset { get { return ArrayOffsets[BaseCTRChunk.IsMajora3D ? VatrChunk.NormalArray_MM : VatrChunk.NormalArray_OoT]; } }
        public float NormalArrayScale { get { return ArrayScales[BaseCTRChunk.IsMajora3D ? VatrChunk.NormalArray_MM : VatrChunk.NormalArray_OoT]; } }
        public Constants.DataTypes NormalArrayDataType { get { return ArrayDataTypes[BaseCTRChunk.IsMajora3D ? VatrChunk.NormalArray_MM : VatrChunk.NormalArray_OoT]; } }

        public uint ColorArrayOffset { get { return ArrayOffsets[BaseCTRChunk.IsMajora3D ? VatrChunk.ColorArray_MM : VatrChunk.ColorArray_OoT]; } }
        public float ColorArrayScale { get { return ArrayScales[BaseCTRChunk.IsMajora3D ? VatrChunk.ColorArray_MM : VatrChunk.ColorArray_OoT]; } }
        public Constants.DataTypes ColorArrayDataType { get { return ArrayDataTypes[BaseCTRChunk.IsMajora3D ? VatrChunk.ColorArray_MM : VatrChunk.ColorArray_OoT]; } }

        public uint TextureCoordArrayOffset { get { return ArrayOffsets[BaseCTRChunk.IsMajora3D ? VatrChunk.TextureCoordArray_MM : VatrChunk.TextureCoordArray_OoT]; } }
        public float TextureCoordArrayScale { get { return ArrayScales[BaseCTRChunk.IsMajora3D ? VatrChunk.TextureCoordArray_MM : VatrChunk.TextureCoordArray_OoT]; } }
        public Constants.DataTypes TextureCoordArrayDataType { get { return ArrayDataTypes[BaseCTRChunk.IsMajora3D ? VatrChunk.TextureCoordArray_MM : VatrChunk.TextureCoordArray_OoT]; } }

        public uint BoneIndexLookupArrayOffset { get { return ArrayOffsets[BaseCTRChunk.IsMajora3D ? VatrChunk.BoneIndexLookupArray_MM : VatrChunk.BoneIndexLookupArray_OoT]; } }
        public float BoneIndexLookupArrayScale { get { return ArrayScales[BaseCTRChunk.IsMajora3D ? VatrChunk.BoneIndexLookupArray_MM : VatrChunk.BoneIndexLookupArray_OoT]; } }
        public Constants.DataTypes BoneIndexLookupArrayDataType { get { return ArrayDataTypes[BaseCTRChunk.IsMajora3D ? VatrChunk.BoneIndexLookupArray_MM : VatrChunk.BoneIndexLookupArray_OoT]; } }

        public uint Unknown_104OoT_120MM { get; private set; }

        public PrmsChunk[] PrmsChunks { get; private set; }

        public VertexPointerType VertexPointerType { get; private set; }
        public int VertexSize { get; private set; }
        public NormalPointerType NormalPointerType { get; private set; }
        public int NormalSize { get; private set; }
        public ColorPointerType ColorPointerType { get; private set; }
        public int ColorSize { get; private set; }
        public TexCoordPointerType TexCoordPointerType { get; private set; }
        public int TexCoordSize { get; private set; }

        public int TotalPrimitives { get; private set; }

        public SepdChunk(byte[] data, int offset, BaseCTRChunk parent)
            : base(data, offset, parent)
        {
            PrmsCount = BitConverter.ToUInt16(ChunkData, 0x08);
            Unknown0A = BitConverter.ToUInt16(ChunkData, 0x0A);
            UnknownFloat0C = BitConverter.ToSingle(ChunkData, 0x0C);
            UnknownFloat10 = BitConverter.ToSingle(ChunkData, 0x10);
            UnknownFloat14 = BitConverter.ToSingle(ChunkData, 0x14);
            Unknown18 = BitConverter.ToUInt32(ChunkData, 0x18);
            Unknown1C = BitConverter.ToUInt32(ChunkData, 0x1C);
            Unknown20 = BitConverter.ToUInt32(ChunkData, 0x20);

            int arrayCount = (BaseCTRChunk.IsMajora3D ? 9 : 8);

            ArrayOffsets = new uint[arrayCount];
            ArrayScales = new float[arrayCount];
            ArrayDataTypes = new Constants.DataTypes[arrayCount];
            ArrayUnknown1 = new ushort[arrayCount];
            ArrayUnknown2 = new uint[arrayCount];
            ArrayUnknown3 = new uint[arrayCount];
            ArrayUnknown4 = new uint[arrayCount];
            ArrayUnknown5 = new uint[arrayCount];

            for (int i = 0; i < arrayCount; i++)
            {
                ArrayOffsets[i] = BitConverter.ToUInt32(ChunkData, 0x24 + (i * 0x1C));
                ArrayScales[i] = BitConverter.ToSingle(BitConverter.GetBytes(BitConverter.ToUInt32(ChunkData, 0x28 + (i * 0x1C))), 0);
                ArrayDataTypes[i] = (Constants.DataTypes)BitConverter.ToUInt16(ChunkData, 0x2C + (i * 0x1C));
                ArrayUnknown1[i] = BitConverter.ToUInt16(ChunkData, 0x2E + (i * 0x1C));
                ArrayUnknown2[i] = BitConverter.ToUInt32(ChunkData, 0x30 + (i * 0x1C));
                ArrayUnknown3[i] = BitConverter.ToUInt32(ChunkData, 0x34 + (i * 0x1C));
                ArrayUnknown4[i] = BitConverter.ToUInt32(ChunkData, 0x38 + (i * 0x1C));
                ArrayUnknown5[i] = BitConverter.ToUInt32(ChunkData, 0x3C + (i * 0x1C));
            }

            Unknown_104OoT_120MM = BitConverter.ToUInt32(ChunkData, !BaseCTRChunk.IsMajora3D ? 0x104 : 0x120);

            int prmsDataOffset = (!BaseCTRChunk.IsMajora3D ? 0x108 : 0x124);

            PrmsChunks = new PrmsChunk[PrmsCount];
            for (int i = 0; i < PrmsChunks.Length; i++) PrmsChunks[i] = new PrmsChunk(ChunkData, (int)BitConverter.ToUInt16(ChunkData, prmsDataOffset + (i * 2)), this);

            switch (NormalArrayDataType)
            {
                case Constants.DataTypes.GL_BYTE:
                case Constants.DataTypes.GL_UNSIGNED_BYTE:
                    NormalPointerType = NormalPointerType.Byte;
                    NormalSize = (sizeof(byte) * 3);
                    break;
                case Constants.DataTypes.GL_SHORT:
                case Constants.DataTypes.GL_UNSIGNED_SHORT:
                    NormalPointerType = NormalPointerType.Short;
                    NormalSize = (sizeof(ushort) * 3);
                    break;
                case Constants.DataTypes.GL_INT:
                case Constants.DataTypes.GL_UNSIGNED_INT:
                    NormalPointerType = NormalPointerType.Int;
                    NormalSize = (sizeof(uint) * 3);
                    break;
                case Constants.DataTypes.GL_FLOAT:
                    NormalPointerType = NormalPointerType.Float;
                    NormalSize = (sizeof(float) * 3);
                    break;
            }

            switch (ColorArrayDataType)
            {
                case Constants.DataTypes.GL_BYTE:
                    ColorPointerType = ColorPointerType.Byte;
                    ColorSize = (sizeof(sbyte) * 4);
                    break;
                case Constants.DataTypes.GL_UNSIGNED_BYTE:
                    ColorPointerType = ColorPointerType.UnsignedByte;
                    ColorSize = (sizeof(byte) * 4);
                    break;
                case Constants.DataTypes.GL_SHORT:
                    ColorPointerType = ColorPointerType.Short;
                    ColorSize = (sizeof(short) * 4);
                    break;
                case Constants.DataTypes.GL_UNSIGNED_SHORT:
                    ColorPointerType = ColorPointerType.UnsignedShort;
                    ColorSize = (sizeof(ushort) * 4);
                    break;
                case Constants.DataTypes.GL_INT:
                    ColorPointerType = ColorPointerType.Int;
                    ColorSize = (sizeof(int) * 4);
                    break;
                case Constants.DataTypes.GL_UNSIGNED_INT:
                    ColorPointerType = ColorPointerType.UnsignedInt;
                    ColorSize = (sizeof(uint) * 4);
                    break;
                case Constants.DataTypes.GL_FLOAT:
                    ColorPointerType = ColorPointerType.Float;
                    ColorSize = (sizeof(float) * 4);
                    break;
            }

            switch (TextureCoordArrayDataType)
            {
                case Constants.DataTypes.GL_BYTE:
                case Constants.DataTypes.GL_UNSIGNED_BYTE:
                    /* Needs conversion to short during rendering! */
                    TexCoordPointerType = TexCoordPointerType.Short;
                    TexCoordSize = (sizeof(byte) * 2);
                    break;
                case Constants.DataTypes.GL_SHORT:
                case Constants.DataTypes.GL_UNSIGNED_SHORT:
                    TexCoordPointerType = TexCoordPointerType.Short;
                    TexCoordSize = (sizeof(ushort) * 2);
                    break;
                case Constants.DataTypes.GL_INT:
                case Constants.DataTypes.GL_UNSIGNED_INT:
                    TexCoordPointerType = TexCoordPointerType.Int;
                    TexCoordSize = (sizeof(uint) * 2);
                    break;
                case Constants.DataTypes.GL_FLOAT:
                    TexCoordPointerType = TexCoordPointerType.Float;
                    TexCoordSize = (sizeof(float) * 2);
                    break;
            }

            switch (VertexArrayDataType)
            {
                case Constants.DataTypes.GL_BYTE:
                case Constants.DataTypes.GL_UNSIGNED_BYTE:
                    /* Needs conversion to short during rendering! */
                    VertexPointerType = VertexPointerType.Short;
                    VertexSize = (sizeof(byte) * 3);
                    break;
                case Constants.DataTypes.GL_SHORT:
                case Constants.DataTypes.GL_UNSIGNED_SHORT:
                    VertexPointerType = VertexPointerType.Short;
                    VertexSize = (sizeof(ushort) * 3);
                    break;
                case Constants.DataTypes.GL_INT:
                case Constants.DataTypes.GL_UNSIGNED_INT:
                    VertexPointerType = VertexPointerType.Int;
                    VertexSize = (sizeof(uint) * 3);
                    break;
                case Constants.DataTypes.GL_FLOAT:
                    VertexPointerType = VertexPointerType.Float;
                    VertexSize = (sizeof(float) * 3);
                    break;
            }

            /* For information purposes */
            TotalPrimitives = 0;
            foreach (PrmsChunk prms in PrmsChunks) TotalPrimitives += (prms.PrmChunk.NumberOfIndices / 3);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("-- {0} --\n", this.GetType().Name);
            sb.AppendFormat(System.Globalization.CultureInfo.InvariantCulture,
                "WARNING: Bunch of unknowns here too...\nNumber of Prms: 0x{0:X}\n" +
                "Vertex array data -> Offset: 0x{1:X}, Scale: {2}, Datatype: {3}\n" +
                "Normal array data -> Offset: 0x{4:X}, Scale: {5}, Datatype: {6}\n" +
                "Color array data -> Offset: 0x{7:X}, Scale: {8}, Datatype: {9}\n" +
                "Tex coord array data -> Offset: 0x{10:X}, Scale: {11}, Datatype: {12}\n" +
                "Bone lookup array data -> Offset: 0x{13:X}, Scale: {14}, Datatype: {15}\n" +
                "(data for other arrays here, don't know them...)\n",
                PrmsCount, VertexArrayOffset, VertexArrayScale, VertexArrayDataType,
                NormalArrayOffset, NormalArrayScale, NormalArrayDataType,
                ColorArrayOffset, ColorArrayScale, ColorArrayDataType,
                TextureCoordArrayOffset, TextureCoordArrayScale, TextureCoordArrayDataType,
                BoneIndexLookupArrayOffset, BoneIndexLookupArrayScale, BoneIndexLookupArrayDataType);
            sb.AppendLine();

            foreach (PrmsChunk prms in PrmsChunks) sb.Append(prms.ToString());

            return sb.ToString();
        }
    }
}
