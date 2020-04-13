using System;
using System.Linq;
using HelpRequestSystem.Services;
using HelpRequestSystem.Data;
using HelpRequestSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace HelpRequestSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            HrsService.Seed();
            Help();
            Console.Write("\n --- --- --- \n");
            bool finish = false;
            do
            {
                char input;
                input = Console.ReadKey(true).KeyChar;
                switch (Char.ToUpper(input))
                {
                    case '1':
                        
                        break;

                    case '2':
                        
                        break;

                    case '3':
                        
                        break;

                    case 'R':
                        ClearDatabase();
                        HrsService.Seed();
                        break;

                    case 'H':
                        Help();
                        break;

                    case 'E':
                        finish = true;
                        break;

                    default:
                        break;
                }

                Console.Write("\n --- --- --- \n");
            } while (!finish);
        }

        public static void Help()
        {
            System.Console.WriteLine("Hit key: " +
                                     "  1: Print open Help Requests for course\n" +
                                     "  2: Print open Help Requests for student\n" +
                                     "  3: Help Requests per course. (Open / Total)\n" +
                                     "  r: Reset Database(takes some time)\n" +
                                     "  h: Help / Print this menu again\n" +
                                     "  e: Exit program");
        }

        public static void ClearDatabase()
        {
            using (var c = new HrsContext())
            {
                c.Database.EnsureDeleted();
                c.Database.Migrate();
            }
        }
    }
}
