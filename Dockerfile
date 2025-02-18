FROM mcr.microsoft.com/dotnet/sdk:9.0
ENV PATH $PATH:/root/.dotnet/tools

WORKDIR /tool
COPY ./ ./

WORKDIR /tool/Trivial.CLI
RUN pwsh -File ./Deploy-Tool.ps1
RUN scaf init

WORKDIR /
RUN rm -rf /tool