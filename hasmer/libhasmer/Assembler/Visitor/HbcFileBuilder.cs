﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Hasmer.Assembler.Parser;

namespace Hasmer.Assembler.Visitor {
    /// <summary>
    /// Represents data for a <see cref="HbcFile"/> that is progressively created by the assembler.
    /// <br />
    /// Once all the data has been added, an HbcFile can be built using the <see cref="Build"/> method.
    /// </summary>
    public class HbcFileBuilder {
        /// <summary>
        /// The file being built.
        /// </summary>
        public HbcFile File { get; set; }

        /// <summary>
        /// The assembler.
        /// </summary>
        private HbcAssembler HbcAssembler { get; set; }

        /// <summary>
        /// Constructs a new HbcFileBuilder.
        /// </summary>
        public HbcFileBuilder(HbcAssembler assembler) {
            HbcAssembler = assembler;
            File.Header = new HbcHeader {
                GlobalCodeIndex = 0,
                Magic = HbcHeader.HBC_MAGIC_HEADER,
                Version = assembler.Header.Format.Version,
                SourceHash = new byte[20],
                Padding = new byte[31],
            };
        }

        public HbcFile Build() {
            File.Header.ArrayBufferSize = (uint)HbcAssembler.DataAssembler.ArrayBuffer.RawBuffer.Count;
            File.Header.ObjKeyBufferSize = (uint)HbcAssembler.DataAssembler.ObjectKeyBuffer.RawBuffer.Count;
            File.Header.ObjValueBufferSize = (uint)HbcAssembler.DataAssembler.ObjectValueBuffer.RawBuffer.Count;
            File.Header.StringCount = (uint)HbcAssembler.DataAssembler.StringTable.Count;

            File.ArrayBuffer = new HbcDataBuffer(HbcAssembler.DataAssembler.ArrayBuffer.RawBuffer.ToArray());
            File.ObjectKeyBuffer = new HbcDataBuffer(HbcAssembler.DataAssembler.ObjectKeyBuffer.RawBuffer.ToArray());
            File.ObjectValueBuffer = new HbcDataBuffer(HbcAssembler.DataAssembler.ObjectValueBuffer.RawBuffer.ToArray());

            return File;
        }
    }
}
