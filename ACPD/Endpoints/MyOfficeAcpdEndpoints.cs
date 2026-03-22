using System.Data;
using System.Text.Json;
using ACPD.Data;
using ACPD.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

namespace ACPD.Endpoints;

public static partial class MyOfficeAcpdEndpoints
{
    public static IEndpointRouteBuilder MapMyOfficeAcpdEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/myofficeacpd", async (MercuryTestDbContext db) =>
        {
            try
            {
                var items = await db.MyOffice_ACPDs.AsNoTracking().ToListAsync();
                return Results.Ok(items);
            }
            catch (Exception ex)
            {
                await TryAddErrorLogAsync(db, "GET /myofficeacpd", new { }, ex);
                return Results.Problem("伺服器內部錯誤", statusCode: StatusCodes.Status500InternalServerError);
            }
        });

        app.MapGet("/myofficeacpd/{sid}", async (string sid, MercuryTestDbContext db) =>
        {
            if (string.IsNullOrWhiteSpace(sid))
            {
                return Results.BadRequest("sid 不可為空");
            }

            try
            {
                var item = await db.MyOffice_ACPDs
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.ACPD_SID == sid);

                return item is null ? Results.NotFound() : Results.Ok(item);
            }
            catch (Exception ex)
            {
                await TryAddErrorLogAsync(db, "GET /myofficeacpd/{sid}", new { sid }, ex);
                return Results.Problem("伺服器內部錯誤", statusCode: StatusCodes.Status500InternalServerError);
            }
        });

        app.MapPost("/myofficeacpd", async (MyOfficeAcpdUpsertRequest input, MercuryTestDbContext db) =>
        {
            if (string.IsNullOrWhiteSpace(input.ACPD_UPDID))
            {
                return Results.BadRequest("ACPD_UPDID 不可為空");
            }

            try
            {
                var newSid = await GetNewSidAsync(db, "MyOffice_ACPD");
                if (string.IsNullOrWhiteSpace(newSid))
                {
                    return Results.Problem("無法由 [dbo].[NEWSID] 取得新 SID。", statusCode: StatusCodes.Status500InternalServerError);
                }

                var now = DateTime.Now;
                var entity = new MyOffice_ACPD
                {
                    ACPD_SID = newSid,
                    ACPD_Cname = input.ACPD_Cname,
                    ACPD_Ename = input.ACPD_Ename,
                    ACPD_Sname = input.ACPD_Sname,
                    ACPD_Email = input.ACPD_Email,
                    ACPD_Status = input.ACPD_Status,
                    ACPD_Stop = input.ACPD_Stop,
                    ACPD_StopMemo = input.ACPD_StopMemo,
                    ACPD_LoginID = input.ACPD_LoginID,
                    ACPD_LoginPWD = input.ACPD_LoginPWD,
                    ACPD_Memo = input.ACPD_Memo,
                    ACPD_NowDateTime = now,
                    ACPD_NowID = input.ACPD_UPDID,
                    ACPD_UPDDateTime = now,
                    ACPD_UPDID = input.ACPD_UPDID
                };

                var exists = await db.MyOffice_ACPDs.AnyAsync(x => x.ACPD_SID == entity.ACPD_SID);
                if (exists)
                {
                    return Results.BadRequest($"ACPD_SID '{entity.ACPD_SID}' already exists.");
                }

                db.MyOffice_ACPDs.Add(entity);
                await db.SaveChangesAsync();

                return Results.Created($"/myofficeacpd/{entity.ACPD_SID}", entity);
            }
            catch (Exception ex)
            {
                await TryAddErrorLogAsync(db, "POST /myofficeacpd", input, ex);
                return Results.Problem("伺服器內部錯誤", statusCode: StatusCodes.Status500InternalServerError);
            }
        });

        app.MapPut("/myofficeacpd/{sid}", async (string sid, MyOfficeAcpdUpsertRequest input, MercuryTestDbContext db) =>
        {
            if (string.IsNullOrWhiteSpace(sid))
            {
                return Results.BadRequest("sid 不可為空");
            }

            if (string.IsNullOrWhiteSpace(input.ACPD_UPDID))
            {
                return Results.BadRequest("ACPD_UPDID 不可為空");
            }

            try
            {
                var item = await db.MyOffice_ACPDs.FirstOrDefaultAsync(x => x.ACPD_SID == sid);
                if (item is null)
                {
                    return Results.NotFound();
                }

                item.ACPD_Cname = input.ACPD_Cname;
                item.ACPD_Ename = input.ACPD_Ename;
                item.ACPD_Sname = input.ACPD_Sname;
                item.ACPD_Email = input.ACPD_Email;
                item.ACPD_Status = input.ACPD_Status;
                item.ACPD_Stop = input.ACPD_Stop;
                item.ACPD_StopMemo = input.ACPD_StopMemo;
                item.ACPD_LoginID = input.ACPD_LoginID;
                item.ACPD_LoginPWD = input.ACPD_LoginPWD;
                item.ACPD_Memo = input.ACPD_Memo;
                item.ACPD_UPDDateTime = DateTime.Now;
                item.ACPD_UPDID = input.ACPD_UPDID;

                await db.SaveChangesAsync();
                return Results.Ok(item);
            }
            catch (Exception ex)
            {
                await TryAddErrorLogAsync(db, "PUT /myofficeacpd/{sid}", new { sid, input }, ex);
                return Results.Problem("伺服器內部錯誤", statusCode: StatusCodes.Status500InternalServerError);
            }
        });

        app.MapDelete("/myofficeacpd/{sid}", async (string sid, MercuryTestDbContext db) =>
        {
            if (string.IsNullOrWhiteSpace(sid))
            {
                return Results.BadRequest("sid 不可為空");
            }

            try
            {
                var item = await db.MyOffice_ACPDs.FirstOrDefaultAsync(x => x.ACPD_SID == sid);
                if (item is null)
                {
                    return Results.NotFound();
                }

                db.MyOffice_ACPDs.Remove(item);
                await db.SaveChangesAsync();

                return Results.NoContent();
            }
            catch (Exception ex)
            {
                await TryAddErrorLogAsync(db, "DELETE /myofficeacpd/{sid}", new { sid }, ex);
                return Results.Problem("伺服器內部錯誤", statusCode: StatusCodes.Status500InternalServerError);
            }
        });

        return app;
    }

    private static async Task<string?> GetNewSidAsync(MercuryTestDbContext db, string tableName)
    {
        await using var command = db.Database.GetDbConnection().CreateCommand();
        command.CommandText = "dbo.NEWSID";
        command.CommandType = CommandType.StoredProcedure;

        var pTableName = command.CreateParameter();
        pTableName.ParameterName = "@TableName";
        pTableName.Value = tableName;
        command.Parameters.Add(pTableName);

        var pReturnSid = command.CreateParameter();
        pReturnSid.ParameterName = "@ReturnSID";
        pReturnSid.Direction = ParameterDirection.Output;
        pReturnSid.DbType = DbType.String;
        pReturnSid.Size = 20;
        command.Parameters.Add(pReturnSid);

        if (command.Connection?.State != ConnectionState.Open)
        {
            await command.Connection!.OpenAsync();
        }

        await command.ExecuteNonQueryAsync();
        return pReturnSid.Value?.ToString();
    }

    private static async Task TryAddErrorLogAsync(MercuryTestDbContext db, string exProgram, object action, Exception ex)
    {
        try
        {
            await using var command = db.Database.GetDbConnection().CreateCommand();
            command.CommandText = "dbo.usp_AddLog";
            command.CommandType = CommandType.StoredProcedure;

            var pReadId = command.CreateParameter();
            pReadId.ParameterName = "@_InBox_ReadID";
            pReadId.Value = (byte)0;
            command.Parameters.Add(pReadId);

            var pSpName = command.CreateParameter();
            pSpName.ParameterName = "@_InBox_SPNAME";
            pSpName.Value = "MyOfficeAcpdEndpoints";
            command.Parameters.Add(pSpName);

            var pGroupId = command.CreateParameter();
            pGroupId.ParameterName = "@_InBox_GroupID";
            pGroupId.Value = Guid.NewGuid();
            command.Parameters.Add(pGroupId);

            var pExProgram = command.CreateParameter();
            pExProgram.ParameterName = "@_InBox_ExProgram";
            pExProgram.Value = exProgram;
            command.Parameters.Add(pExProgram);

            var pActionJson = command.CreateParameter();
            pActionJson.ParameterName = "@_InBox_ActionJSON";
            pActionJson.Value = JsonSerializer.Serialize(new
            {
                Action = action,
                Exception = ex.Message,
                StackTrace = ex.StackTrace
            });
            command.Parameters.Add(pActionJson);

            var pOutput = command.CreateParameter();
            pOutput.ParameterName = "@_OutBox_ReturnValues";
            pOutput.Direction = ParameterDirection.Output;
            pOutput.Size = -1;
            pOutput.DbType = DbType.String;
            command.Parameters.Add(pOutput);

            if (command.Connection?.State != ConnectionState.Open)
            {
                await command.Connection!.OpenAsync();
            }

            await command.ExecuteNonQueryAsync();
        }
        catch
        {
        }
    }

}
