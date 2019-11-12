<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Marka.aspx.cs" Inherits="Marka" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
  

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Icerik" Runat="Server">
    <div class="page-header">
        <div class="container">
            <div class="banner relative" style="background: url(/img/page-back.jpg) no-repeat center center; height: 230px; padding:0; background-size:cover; width: 100%;">
                <img src="img/arcelik.jpg" alt="Marka Adı" class="brands"/>
            </div>
        </div>

    </div>

   <div class="container">
        <div class="row">
            <div class="col-md-12 tab m-t-b-30">
                 <ul class="nav nav-tabs" role="tablist">
                    <li role="presentation" class="active"><a href="#one" aria-controls="one" role="tab" data-toggle="tab">Ankastre</a></li>
                    <li role="presentation"><a href="#two" aria-controls="two" role="tab" data-toggle="tab">Beyaz Eşya</a></li>
                    <li role="presentation"><a href="#three" aria-controls="three" role="tab" data-toggle="tab">Elektronik</a></li>
                 </ul>
               
                <div class="tab-content">
                  <div role="tabpanel" class="tab-pane fade in active" id="one">
                         <div class="col-md-3 col-sm-4 col-xs-6">
                <a class="brand" href="https://www.arcelik.com.tr/buzdolabi/" target="_blank">
                    <img src="img/alayli.jpg" alt="Marka Adı"/>
                    <span>Buzdolapları</span>
                </a>
            </div>
                  </div>
                  <div role="tabpanel" class="tab-pane fade" id="two">

                  </div>

                 <div role="tabpanel" class="tab-pane fade" id="three">
                </div>
                </div>  

            </div>
        </div>
    </div>


</asp:Content>

