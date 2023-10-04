# Sistema de Gerenciamento de Relatórios de Folha de Ponto

## Introdução

Este projeto tem como objetivo desenvolver um sistema capaz de automatizar a geração de relatórios de folha de ponto para o departamento de Recursos Humanos. O sistema irá processar arquivos CSV, calcular os dados da folha de ponto e gerar um arquivo JSON consolidado para facilitar o processo de pagamento dos funcionários.

## Objetivos

O objetivo principal deste projeto é proporcionar uma solução eficiente para a geração de relatórios de folha de ponto, substituindo o processo manual anteriormente realizado no Excel.

## Funcionalidades Principais

- Leitura de vários arquivos CSV de um diretório de rede especificado pelo usuário.
- Utilização de paralelismo e métodos assíncronos para otimizar o processamento dos arquivos.
- Cálculo diário dos dados da folha de ponto, incluindo verificação de dias úteis, faltas, horas extras, entre outros.
- Consolidação dos dados por funcionário e geração de um arquivo JSON.
- Suporta até 10 milhões de linhas de dados para garantir desempenho.

## Requisitos Técnicos

- Plataforma: .NET 6
- Arquitetura: Domain Driven Design (DDD)
- Biblioteca: Newtonsoft.Json v13.0.3
- Testes: xUnit v2.4.2

## Escopo de Execução

O escopo deste projeto inclui:

- Desenvolvimento das funcionalidades principais descritas acima.
- Utilização de DDD para uma arquitetura bem estruturada.

O escopo deste projeto exclui:

- Persistência de dados em um banco de dados.
- Geração e armazenamento de logs de atividades.
- Implementação de autenticação de usuários.

## Dependências

- Newtonsoft.Json v13.0.3
- xUnit v2.4.2
- xunit.runner.visualstudio v2.4.5

## Execução do projeto

- É necessário ter instalado o SKD do .Net 6
- O projeto pode ser executado no Visual Studio Code, ou em outras IDEs como: Visual Studio 2022 e Rider.