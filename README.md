# Notes App

**Descrição do projeto:** O Notes App é um sistema de gerenciamento de notas desenvolvido utilizando ASP.NET MVC e MySQL.

## Requisitos:
- .NET 8
- MySQL Server

## Como executar o projeto:

**1.** Primeiramente, clone este repositório em sua máquina e abra o projeto no Visual Studio.

**2.** Acesse o Console de Gerenciador de Pacotes no Visual Studio seguindo o caminho:

**Ferramentas (Tools) -> Gerenciador de Pacotes NuGet (NuGet Package Manager) -> Console de Gerenciador de Pacotes (Package Manager Console)**

Em seguida, execute o seguinte comando para restaurar os pacotes do projeto:

    dotnet restore

**3.** Após isso, digite o comando abaixo para listar os pacotes instalados:

    Get-Package | Format-List

**4.** Verifique se os seguintes pacotes estão instalados:

![NuGet Packages](https://github.com/user-attachments/assets/75d90129-548b-405c-bf79-600710e3d324)

_**Observação:** Caso algum dos pacotes listados esteja instalado no seu Visual Studio em uma versão diferente das indicadas, teste os próximos passos normalmente. Se ocorrer algum erro devido à incompatibilidade de versão, desinstale apenas os pacotes que apresentaram erro utilizando o comando:_

_Uninstall-Package NomeDoPacote_

_Exemplo:_

_Uninstall-Package Pomelo.EntityFrameworkCore.MySql_

_Em seguida, reinstale as versões corretas dos pacotes desinstalados conforme indicado no passo 5._

**5.** Se todos os pacotes estiverem instalados conforme a imagem do passo 4, vá para o passo 6. Em caso negativo, instale apenas os pacotes faltantes com os seguintes comandos:

Pacote 1:

    Install-Package Microsoft.VisualStudio.Web.CodeGeneration.Design -Version 8.0.7

Pacote 2:

    Install-Package Pomelo.EntityFrameworkCore.MySql -Version 8.0.3

Pacote 3:

    Install-Package Microsoft.EntityFrameworkCore.Tools -Version 8.0.11

**6.** Abra o arquivo appsettings.json e configure a conexão com o banco de dados, alterando os parâmetros necessários, como a senha (pwd) e outros dados relevantes de acordo com o seu usuário do MySQL.

![appsettings.js](https://github.com/user-attachments/assets/2c18d40a-6c17-4a80-863c-b96b54922373)

**7.** No arquivo Program.cs, verifique se a versão do MySQL configurada corresponde à instalada em sua máquina.

![MySQL Version in Program.cs](https://github.com/user-attachments/assets/3c30f530-4a35-4238-b028-b6ec5d7aa266)

Para verificar a versão do MySQL instalada, abra o MySQL Workbench e siga os passos:

![MySQL Workbench](https://github.com/user-attachments/assets/d15ca31b-e6c1-4755-b191-9d214be674ee)

![MySQL Version in MySQL Workbench](https://github.com/user-attachments/assets/916f64bb-b9f4-4817-9b80-00be590c7e97)

Se necessário, ajuste a versão no código do projeto conforme sua instalação.

**8.** Certifique-se de que o MySQL Server está em execução: Abra o **Gerenciador de Tarefas**, vá até a guia **Serviços**, localize o serviço "MySQL" e verifique se o status está como "Em execução". Caso não esteja, clique com o botão direito do mouse sobre "MySQL" e selecione "Iniciar".

**9.** Novamente, acesse o Console de Gerenciador de Pacotes no Visual Studio seguindo o caminho:

**Ferramentas (Tools) -> Gerenciador de Pacotes NuGet (NuGet Package Manager) -> Console de Gerenciador de Pacotes (Package Manager Console)**

Em seguida, execute o seguinte comando para aplicar as migrações ao banco de dados:

    Update-Database -Context NotesContext

**10.** Por fim, compile e execute o projeto no Visual Studio.
