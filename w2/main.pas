unit main;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, StdCtrls, Menus, ShellAPI, ExtCtrls;

type
  TForm1 = class(TForm)
    Button1: TButton;
    ListBox1: TListBox;
    Edit1: TEdit;
    Edit2: TEdit;
    Label1: TLabel;
    Label2: TLabel;
    Button2: TButton;
    Label3: TLabel;
    Label4: TLabel;
    Edit3: TEdit;
    ListBox2: TListBox;
    Label5: TLabel;
    MainMenu1: TMainMenu;
    File1: TMenuItem;
    About1: TMenuItem;
    Close1: TMenuItem;
    Add1: TMenuItem;
    Readfromfile1: TMenuItem;
    OpenDialog1: TOpenDialog;
    Button3: TButton;
    Label6: TLabel;
    Label7: TLabel;
    Addfromfile1: TMenuItem;
    Button4: TButton;
    Button5: TButton;
    Button6: TButton;
    Label8: TLabel;
    Label9: TLabel;
      
    procedure AddToList(_m, _s: string);

    procedure Button1Click(Sender: TObject);
    procedure FormCreate(Sender: TObject);
    procedure Close1Click(Sender: TObject);
    procedure About1Click(Sender: TObject);
    procedure Readfromfile1Click(Sender: TObject);
    procedure Button3Click(Sender: TObject);
    procedure Addfromfile1Click(Sender: TObject);
    procedure Button2Click(Sender: TObject);
    procedure Button4Click(Sender: TObject);
  private
    { Private declarations }
  public
    { Public declarations }
  end;

var
  Form1: TForm1;
  m, s, x: Array[0..101] of Real;
  count: integer;

implementation

{$R *.dfm}

function CheckField(field: TEdit): boolean;
begin
    if field.Text = '' then
    begin
        ShowMessage('Enter integer or real number in field');
        result := false;
    end
    else
        result := true;
end;

procedure TForm1.AddToList(_m, _s: string);
begin
    m[count] := StrToFloat(_m);
    s[count] := StrToFloat(_s);
    ListBox1.Items.Add(IntToStr(count + 1) + '. m = ' +
    _m + ';   s = ' + _s + ';');
    count := count + 1;
end;

procedure TForm1.Button1Click(Sender: TObject);
begin
    if count = 100 then
    begin
        ShowMessage('Too much data');
        Exit;
    end;

    if CheckField(Edit1) and CheckField(Edit2) then
    begin
        AddToList(Edit1.Text, Edit2.Text);
        Edit1.Clear;
        Edit2.Clear;
    end;
end;

procedure TForm1.FormCreate(Sender: TObject);
begin
    count := 0;
end;

procedure TForm1.Close1Click(Sender: TObject);
begin
    Application.Terminate;
end;

procedure TForm1.About1Click(Sender: TObject);
begin
    ShellAbout(Form1.Handle, 'Markowitz Model',
    'Markowitz Model program.' + #13#10 +
    'Author: %yourname%', Application.Icon.Handle);
end;

procedure TForm1.Readfromfile1Click(Sender: TObject);
var
    _m, _s: Real;
    f: TextFile;

begin
    if openDialog1.Execute then
    begin
        ListBox1.Clear;
        count := 0;
        AssignFile(f, openDialog1.FileName);
        Reset(f);

        while not EOF(f) do
        begin
            read(f, _m);
            read(f, _s);
            AddToList(FloatToStr(_m), FloatToStr(_s));
        end;
        CloseFile(f);
    end;
end;

procedure TForm1.Button3Click(Sender: TObject);
begin
    ListBox1.Clear;
    count := 0;
end;

procedure TForm1.Addfromfile1Click(Sender: TObject);
var
    _m, _s: Real;
    f: TextFile;

begin
    if openDialog1.Execute then
    begin
        AssignFile(f, openDialog1.FileName);
        Reset(f);

        while not EOF(f) do
        begin
            read(f, _m);
            read(f, _s);
            AddToList(FloatToStr(_m), FloatToStr(_s));
        end;
        CloseFile(f);
    end;
end;

procedure TForm1.Button2Click(Sender: TObject);
var
    i: Integer;
    d: Real;

begin
    d := 0;
    ListBox2.Clear;
    for i := 0 to count - 1 do
    begin
        d := d + m[i] / s[i];
        x[i] := d;
        ListBox2.Items.Add(IntToStr(i + 1) + '. x = ' + FloatToStr(x[i]) + ';');
    end;
end;

procedure TForm1.Button4Click(Sender: TObject);
begin
    if count > 0 then
    begin
        ListBox1.Items.Delete(count - 1);
        count := count - 1;
    end;
end;

end.
