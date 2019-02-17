namespace GeneratorConsole
{
    using System;
    using System.IO;
    using System.Text;


    class Program
    {
        public static readonly string SPropTemplate = File.ReadAllText(@"sproptemplate.txt");

        public static readonly string SFactTemplate = File.ReadAllText(@"PropertyTypeFactoryTemplate.txt");
        static void Main(string[] args)
        {

            var types = new[]
                            {
                                typeof(bool), typeof(DateTime), typeof(int), typeof(short), typeof(long),
                                typeof(decimal), typeof(double), typeof(float), typeof(Guid), typeof(string),
                                typeof(char), typeof(byte)
                            };


            if (args[0] == "Props")
            {
                StringBuilder sb = new StringBuilder();
                foreach (var t in types)
                {
                    var name = t.Name;
                    sb.Append(SPropTemplate.Replace("{DotNetName}", name));
                    sb.AppendLine();
                }

                Console.Write(sb.ToString());
            }
            else if (args[0] == "Fact")
            {
                
                var sb = new StringBuilder();
                foreach (var t in types)
                {
                    var name = t.Name;
                    sb.Append(SFactTemplate.Replace("{DotNetType}", name));
                    sb.AppendLine();
                }

                Console.Write(sb.ToString());

            }
            else
            {
                Console.WriteLine("Must enter Fact or Props");
            }
            var x = Console.ReadLine();
        }
    }


}
