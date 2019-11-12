<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="GaleriDetay.aspx.cs" Inherits="GaleriDetay" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
  
         <link href="css/lightbox.min.css" rel="stylesheet" />
         <link href="css/lightgallery.css" rel="stylesheet" />

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Icerik" Runat="Server">
    <div class="page-header">
        <div class="container">
            <div class="banner page-back relative">
                <h1>GALERİ ADI</h1>
            </div>
        </div>
    </div>

    <div class="container">
        <div class="row">
             <div class="col-md-3 col-sm-3 col-xs-12">
                            <a href="img/galeri.jpg" class="brand" data-lightbox="example-set" data-title="Albüm Adı">
                            <img class="example-image" data-="Albüm Adı" alt="Albüm Adı" src="img/galeri.jpg"/>
                        </a>
                </div> 

           <div class="col-md-3 col-sm-3 col-xs-12">
                            <a href="img/galeri.jpg" class="brand" data-lightbox="example-set" data-title="Albüm Adı">
                            <img class="example-image" data-="Albüm Adı" alt="Albüm Adı" src="img/galeri.jpg"/>
                        </a>
                </div> 

<div class="col-md-3 col-sm-3 col-xs-12">
                            <a href="img/galeri.jpg" class="brand" data-lightbox="example-set" data-title="Albüm Adı">
                            <img class="example-image" data-="Albüm Adı" alt="Albüm Adı" src="img/galeri.jpg"/>
                        </a>
                </div> 

<div class="col-md-3 col-sm-3 col-xs-12">
                            <a href="img/galeri.jpg" class="brand" data-lightbox="example-set" data-title="Albüm Adı">
                            <img class="example-image" data-="Albüm Adı" alt="Albüm Adı" src="img/galeri.jpg"/>
                        </a>
                </div> 

         
        </div>

    </div>

    
 <script src="js/lightbox-plus-jquery.min.js"></script> 
</asp:Content>

