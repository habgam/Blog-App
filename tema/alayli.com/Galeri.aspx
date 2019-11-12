<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Galeri.aspx.cs" Inherits="Galeri" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
  
         <link href="css/lightbox.min.css" rel="stylesheet" />
         <link href="css/lightgallery.css" rel="stylesheet" />

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Icerik" Runat="Server">
    <div class="page-header">
        <div class="container">
            <div class="banner page-back relative">
                <h1>GALERİ</h1>
            </div>
        </div>
    </div>

    <div class="container">
        <div class="row">
            <div class="col-md-3 col-sm-4 col-xs-6">
                <a class="brand" href="GaleriDetay.aspx">
                    <img src="img/galeri.jpg" alt="Marka Adı"/>
                    <span>Albüm Adı</span>
                </a>
            </div>
           
         
        </div>

    </div>

</asp:Content>

