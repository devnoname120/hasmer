﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hasmer.Assembler {
    /// <summary>
    /// Represents a Hermes bytecode disassembler.
    /// </summary>
    public class HbcDisassembler {
        /// <summary>
        /// The source bytecode file for disassembly.
        /// </summary>
        public HbcFile Source { get; }

        /// <summary>
        /// If true, preserve the original instructions and do not abstract out variant instructions.
        /// <br />
        /// If false, instructions will be converted to their variants.
        /// <br />
        /// See <see cref="HbcAbstractInstructionDefinition"/> for more information about variant instructions and abstraction.
        /// </summary>
        public bool IsExact { get; }

        /// <summary>
        /// The disassembler for data. Data is disassembled separately from code, and this property is the disassembler object.
        /// </summary>
        public DataDisassembler DataDisassembler { get; private set; }

        /// <summary>
        /// Created a new disassembler given a bytecode file.
        /// </summary>
        /// <param name="source">The bytecode file to disassemble.</param>
        public HbcDisassembler(HbcFile source, bool isExact) {
            Source = source;
            IsExact = isExact;
            DataDisassembler = new DataDisassembler(source);
        }

        /// <summary>
        /// Disassembles the bytecode file.
        /// </summary>
        /// <returns>A string representing the Hasm disassembly.</returns>
        public string Disassemble() {
            StringBuilder builder = new StringBuilder();
            builder.Append(".hasm ");
            builder.AppendLine(Source.Header.Version.ToString());
            builder.AppendLine();
            builder.AppendLine(DataDisassembler.Disassemble());

            foreach (HbcSmallFuncHeader func in Source.SmallFuncHeaders) {
                FunctionDisassembler decompiler = new FunctionDisassembler(this, func.GetAssemblerHeader());
                builder.AppendLine(decompiler.Disassemble());
                builder.AppendLine();
            }

            return builder.ToString();
        }
    }
}
