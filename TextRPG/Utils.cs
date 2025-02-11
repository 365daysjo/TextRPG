﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Tool
{
    public static class Utils
    {
        private static List<string> previousContent = new List<string>();

        /// <summary>
        /// 이전 장면과 구분할 수 있는 선 추가
        /// 장면의 이름과 설명 출력 가능
        /// </summary>
        /// <param name="title">장면의 이름 (없을 경우 "")</param>
        /// <param name="description">장면의 설명 (없을 경우 "")</param>
        public static void ShowHeader(string title, string description, ConsoleColor color = ConsoleColor.DarkCyan)
        {
            Console.WriteLine("\n========================================================================================================================");
            Thread.Sleep(500);

            Console.ForegroundColor = color;
            if (title != "") Console.WriteLine($"\n{title}");

            Console.ForegroundColor = ConsoleColor.DarkGray;
            if (description != "") Console.WriteLine($"{description}\n");
            Console.ResetColor();
        }

        /// <summary>
        /// 선택지 스타일을 간편하게 구현
        /// </summary>
        /// <param name="index">선택지의 번호</param>
        /// <param name="name">선택지 이름</param>
        public static void OptionText(int index, string name, ConsoleColor color = ConsoleColor.DarkCyan)
        {
            Console.ForegroundColor = color;
            Console.Write(index);
            Console.ResetColor();
            Console.WriteLine($". {name}");
        }

        /// <summary>
        /// 플레이어 스타일을 간편하게 구현
        /// </summary>
        public static void PlayerText(Character player)
        {
            Console.Write("Lv.");

            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write(player.Level);
            Console.ResetColor();

            Console.Write($"  {player.Name} ");
            string playerClass = Utils.FormatString($"({player.Class})", 13 - Utils.GetDisplayWidth(player.Name));

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write(playerClass);
            Console.ResetColor();

            Console.Write($"(공격력 ");

            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write(player.TotalAttack);
            Console.ResetColor();

            Console.Write($" / 체력 ");

            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write(player.Health);
            Console.ResetColor();

            Console.WriteLine(")");
        }

        /// <summary>
        /// 몬스터 스타일을 간편하게 구현
        /// </summary>
        /// <param name="level">몬스터 레벨</param>
        /// <param name="name">몬스터 이름</param>
        /// <param name="attack">몬스터 공격력</param>
        /// <param name="health">몬스터 체력</param>
        /// <param name="isDead">몬스터 생사 여부</param>
        public static void MonsterText(int level, string name, float attack, int health, bool isDead)
        {
            name = FormatString(name, 14);

            if (!isDead)
            {
                Console.Write("Lv.");

                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.Write(level);
                Console.ResetColor();

                if (level <= 9)
                    Console.Write($"  {name}(공격력 ");
                else
                    Console.Write($" {name}(공격력 ");

                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.Write(attack);
                Console.ResetColor();

                Console.Write($" / 체력 ");

                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.Write(health);
                Console.ResetColor();

                Console.WriteLine(")");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine($"Lv.{level} {name} (공격력 {attack} / 체력 {health})");
                Console.ResetColor();
            }
        }

        /// <summary>
        /// 회색으로 표시될 안내 메시지에 사용
        /// </summary>
        /// <param name="text">표시할 텍스트</param>
        public static void InfoText(string text)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(text);
            Console.ResetColor();
        }

        /// <summary>
        /// 빨간색으로 표시될 경고 메시지에 사용
        /// </summary>
        /// <param name="text">표시할 텍스트</param>
        public static void ErrorText(string text)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(text);
            Console.ResetColor();
        }

        /// <summary>
        /// 인풋 값의 허용 범위를 정하고, 이외의 입력에는 "잘못된 입력" 경고
        /// (min, max 값도 범위에 포함)
        /// </summary>
        /// <param name="min">최소 허용 범위</param>
        /// <param name="max">최대 허용 범위</param>
        /// <returns></returns>
        public static int GetInput(int min, int max)
        {
            while (true)
            {
                Console.WriteLine("\n원하시는 행동을 입력해주세요.");

                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.Write(">> ");

                if (int.TryParse(Console.ReadLine(), out int input) && input >= min && input <= max)
                {
                    Console.ResetColor();
                    return input;
                }

                ErrorText("잘못된 입력입니다. 다시 시도해주세요.");
            }
        }

        /// <summary>
        /// 콘솔에 출력될 글자가 실제로 몇칸인지 측정
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static int GetDisplayWidth(string text)
        {
            int width = 0;
            foreach (char c in text)
            {
                // 한글은 2칸, 나머지는 1칸
                width += (c >= 0xAC00 && c <= 0xD7A3) ? 2 : 1;
            }
            return width;
        }

        /// <summary>
        /// 텍스트의 오른쪽에 공백을 추가해서 특정 길이에 맞춤
        /// </summary>
        /// <param name="text">적용할 텍스트</param>
        /// <param name="totalWidth">총 길이</param>
        /// <returns></returns>
        public static string FormatString(string text, int totalWidth)
        {
            int padding = totalWidth - GetDisplayWidth(text);
            return text + new string(' ', padding > 0 ? padding : 0);
        }

        public static void FadePreviousContent()
        {
            // 기존 출력 내용 회색으로 변경
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkGray;
            foreach (var line in previousContent)
            {
                Console.WriteLine(line);
            }
            Console.ResetColor();

            // 기존 내용 저장 후 초기화
            previousContent.Clear();
        }

        public static void StoreContent(string content)
        {
            previousContent.Add(content);
        }
    }
}
