using BooksLibrary.BL;
using BooksLibrary.DB;
using Ninject.Modules;


namespace BooksLibrary.UI
{
    class SimpleConfigModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IBooksLibraryDBContext>().To<BooksLibraryDBContext>();
            Bind<BooksLibraryDBContext>().ToSelf();
            Bind<IControlerDB>().To<ControlerDB>();
            Bind<ControlerDB>().ToSelf();
        }
    }
}
