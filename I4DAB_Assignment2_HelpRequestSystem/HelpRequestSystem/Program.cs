using System;
using System.Collections.Generic;
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
            Console.Write(" --- --- --- \n");
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
                        Console.Write("Enter student ID:");
                        var temp = Console.ReadLine();

                        if (HrsService.GetHelpRequestsForStudent(int.Parse(temp),
                            out List<Exercise> exercises,
                            out List<Assignment> assignments))
                        {
                            foreach (var exercise in exercises)
                            {
                                Console.WriteLine($"{exercise.Number} {exercise.Lecture} {exercise.Teacher} {exercise.HelpWhere} Is Help Request open:{exercise.IsOpen}");
                            }

                            foreach (var assignment in assignments)
                            {
                                Console.WriteLine($"{assignment.AssignmentName} Is Help Request open:{assignment.IsOpen}");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Unknown student id");
                        }

                        break;

                    case '3':
                        
                        break;

                    case 'R':
                        Console.WriteLine("Clearing database...");
                        ClearDatabase();
                        Console.WriteLine("ReSeeding database...");
                        HrsService.Seed();
                        Console.WriteLine("Done!");
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

                Console.Write(" --- --- --- \n");
            } while (!finish);
        }

        public static void Help()
        {
            System.Console.Write("Hit key:\n" +
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
