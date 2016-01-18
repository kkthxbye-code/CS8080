﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CS8080
{
    public class State
    {
        public Memory memory = new Memory(64 * 1000);
        public Registers registers;
        public Instructions instructions = new Instructions();
        public Stack stack = new Stack();
        public byte currentOpcode = 0;

        public int sp = 0;
        public int cycleCount = 0;

        public void CallInstruction(byte instruction)
        {
            try
            {
                instructions.instructions[instruction](this);
            } catch(KeyNotFoundException)
            {
                Console.WriteLine("Instruction not implemented: H: 0x{0:X}", instruction);
                registers.SetFlags((byte) Flag.ZERO);
                DumpState();

                Console.ReadLine();
                System.Environment.Exit(1);
            }
        }

        public void LoadRom(string path)
        {
            File.ReadAllBytes(path).CopyTo(memory.memory, 0);
        }

        public void Run()
        {
            registers = new Registers(this);
            while (true)
            {
                NextInstruction();
            }
        }

        public void NextInstruction()
        {
            byte opcode = memory.ReadByte();
            currentOpcode = opcode;
            //Console.WriteLine("0x{0:X}", opcode);
            CallInstruction(opcode);
        }

        public void DumpState()
        {
            registers.DumpRegisters();
            stack.DumpStackPointer();
            registers.DumpFlags();
            Console.WriteLine("PC: 0x{0:X}", memory.pc);
        }
    }
}