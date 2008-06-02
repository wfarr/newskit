using System;

using Mono.Unix;

using NDesk.DBus;

namespace NewsKit.Dbus
{
    public class DBusInterface ()
    {
        public static void Main ()
        {
            Console.WriteLine ("[started] NewsKit.DBusInterface.Main");
            Console.WriteLine ("I'm an instance of DBusInterface dude!");
            Bus bus = Bus.Session;
            Console.WriteLine ("[finished] NewsKit.DBusInterface.Main");
        }

        public static void LoadFeed ()
        {
            Console.WriteLine ("[started] NewsKit.DBusInterface.LoadFeed");
            Console.WriteLine ("I'm loading a feed dude!");
            Console.WriteLine ("[finished] NewsKit.DBusInterface.LoadFeed");
        }

        public static void Update ()
        {
            Console.WriteLine ("[started] NewsKit.DBusInterface.Update");
            Console.WriteLine ("I'm updating stuff dude!");
            Console.WriteLine ("[finished] NewsKit.DBusInterface.Update");
        }

        public static void NewItems ()
        {
            Console.WriteLine ("[started] NewsKit.DBusInterface.NewItems");
            Console.WriteLine ("[finished] NewsKit.DBusInterface.NewItems");
            return 1;
        }
    }
}