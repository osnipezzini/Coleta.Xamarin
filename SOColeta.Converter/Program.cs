var filename = @"C:\Users\osnip\OneDrive\Área de Trabalho\inventario.txt";

var arquivoString = string.Empty;

var nomeArquivo = $"Inventario_Opticon.txt";
var arquivo = Path.Combine(@"C:\Users\osnip\OneDrive\Área de Trabalho", nomeArquivo);
foreach (var line in File.ReadAllLines(filename))
{
    var codigo = line.Split(";")[0];
    var quantidade = Convert.ToInt32(line.Split(";")[1]).ToString().PadLeft(6, '0');
    var linhaString = $"{codigo.PadLeft(14, '0')}{quantidade}0000000{DateTime.Now.ToString("dd/MM/yyHH:mm:ss")}\n";
    if (linhaString.Length == 44)
        arquivoString += linhaString;
}

Console.WriteLine(arquivoString);
File.WriteAllText(arquivo, arquivoString);