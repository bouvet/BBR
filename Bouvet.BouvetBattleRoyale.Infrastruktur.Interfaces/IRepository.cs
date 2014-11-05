namespace Bouvet.BouvetBattleRoyale.Infrastruktur.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IRepository<T>
    {
        String CollectionId { get; }

        Task<string> Opprett(T document);

        IEnumerable<T> HentAlle();

        T Hent(string id);

        Task Oppdater(T document);

        Task Slett(T document);

        IEnumerable<T> S�k(Func<T, bool> predicate);
    }
}