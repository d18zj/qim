using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Qim.EntitiFrameworkCore.Tests.Domain;
using Xunit;

namespace Qim.EntitiFrameworkCore.Tests.Test
{
    public class SeqGuidTests : TestBase
    {
        [Fact]
        [SuppressMessage("ReSharper", "RedundantStringFormatCall")]
        public async Task SeqGuid_Test()
        {
            await UsingDbContextAsync(async context =>
            {
                await context.Database.ExecuteSqlCommandAsync("TRUNCATE TABLE public.\"Test_SeqGuid\"");
                int maxCount = 10000;
                List<SeqGuid> seqGuids = new List<SeqGuid>();
                var allGuids = new List<Guid>();
                for (int i = 0; i < maxCount; i++)
                {
                    var strKey = SequentialGuidGenerator.Instance.NewGuid();
                    var guidKey = SequentialGuidGenerator.Instance.NewGuid(SequentialGuidType.SequentialAtEnd);
                    var arrKey = SequentialGuidGenerator.Instance.NewGuid(SequentialGuidType.SequentialAsBinary);
                    seqGuids.Add(new SeqGuid
                    {
                        PId = i + 1,
                        StrKey = strKey.ToString("N"),
                        GuidKey = guidKey,
                        ArrKey = arrKey.ToByteArray()
                    });
                    allGuids.Add(strKey);
                    allGuids.Add(guidKey);
                    allGuids.Add(arrKey);
                }

                await context.SeqGuids.AddRangeAsync(seqGuids);
                await context.SaveChangesAsync();

                var haseSet = new HashSet<Guid>(allGuids);

                Assert.Equal(allGuids.Count, haseSet.Count);
                Assert.Equal(maxCount, await context.SeqGuids.CountAsync());

                string sql = @"select * from 
                            (select ROW_NUMBER() over (order by  ""{0}"" desc) as Row,* from public.""Test_SeqGuid"" ) a
                            where a.""row""+a.""PId""<>" + (maxCount + 1);


                //测试字符串形式
                var list = await context.SeqGuids.FromSql(string.Format(sql, "StrKey")).ToListAsync();
                Assert.Empty(list);

                //测试Guid(uniqueidentifier类型)形式
                list = await context.SeqGuids.FromSql(string.Format(sql, "GuidKey")).ToListAsync();
                Assert.Empty(list);

                //测试二进制形式
                list = await context.SeqGuids.FromSql(string.Format(sql, "ArrKey")).ToListAsync();
                Assert.Empty(list);
            });
        }
    }
}