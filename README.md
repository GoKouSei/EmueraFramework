This is Cross-Platform Framework based on Emuera(https://ko.osdn.jp/projects/emuera/)

----------------------------------------------------------------------------------------------------------------------------------------


Example

<pre><code>

@SYSTEM_TITLE
CALL SETFLAG, 1
RETURN

public void SETFLAG(long arg)
{
  Main.Framework.Data["FLAG"] = arg;
  Main.Framework.Print(Main.Framework.Data["FLAG"].ToString(), PrintFlags.NEWLINE);
}

Flow

CALL SETFLAG, 1
arg set 1

Main.Framework.Data["FLAG"] = arg
Set Flag:0 to arg

Main.Framework.Print(Main.Framework.Data["FLAG"].ToString(), PrintFlags.NEWLINE);
Print FLAG:0 with NEWLINE

RETURN
End Function

</code></pre>
