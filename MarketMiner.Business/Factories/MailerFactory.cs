using MarketMiner.Business.Contracts;
using N.Core.Common.Core;
using P.Core.Common.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

namespace MarketMiner.Business
{
   [Export(typeof(IMailerFactory))]
   [PartCreationPolicy(CreationPolicy.NonShared)]
   public class MailerFactory : IMailerFactory
   {
      T IMailerFactory.GetMailer<T>()
      {
         T mailer = ObjectBase.Container.GetExports<T>().FirstOrDefault().Value;

         return mailer;
      }

      /// <summary>
      /// Gets a mailer from the types container.
      /// </summary>
      /// <param name="assemblyQualifiedName">The assembly-qualified name of the type.</param>
      /// <returns>An IMailer object or null if a mathcing object is not found.</returns>
      IMailer IMailerFactory.GetMailer(string assemblyQualifiedName)
      {
         Type type = Type.GetType(assemblyQualifiedName);
         IMailer mailer = ObjectBase.Container.GetExportedValue<IMailer>(type.Name);

         return mailer;
      }
   }
}
