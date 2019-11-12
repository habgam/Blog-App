<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Iletisim.aspx.cs" Inherits="Iletisim" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Icerik" Runat="Server">
    <div class="page-header">
        <div class="container">
            <div class="banner page-back relative">
                <h1>İLETİŞİM</h1>
            </div>
        </div>
    </div>

    <div class="container contact">
        <div class="row">
            <div class="col-md-6 col-xs-12 p-30">
                <h2>MERKEZ MAĞAZA</h2>
                <p>
                    Sakarya Cad. No:58 <br/>
                    Adapazarı / SAKARYA <br/>
                    <strong>Telefon:</strong> 0 264 278 22 30<br/>
                    <strong>E-Posta:</strong><br/>
                    <a href="mailto:info@alayli.com.tr">info@alayli.com.tr</a> <br/>
                </p>
                <iframe src="https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d3021.614898826386!2d30.405223315319333!3d40.77049404197195!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x14ccb336b6ed1b19%3A0xa2c2519e77bcdf0e!2sAlayl%C4%B1+Ticaret!5e0!3m2!1str!2str!4v1499980832792" width="100%" height="450" class="m-t-b-30" frameborder="0" style="border:0;" allowfullscreen></iframe>
            </div>
            <div class="col-md-6 col-xs-12 p-30">
                <h2>ŞUBE</h2>
                 <p>
                    Atatürk Bulvarı No:58 <br/>
                    Adapazarı / SAKARYA <br/>
                    <strong>Telefon:</strong> 0 264 278 09 78<br/>
                    <strong>E-Posta:</strong><br/>
                    <a href="mailto:info@alayli.com.tr">info@alayli.com.tr</a> <br/>
                </p>
                 <iframe src="https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d3021.614898826386!2d30.405223315319333!3d40.77049404197195!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x14ccb336b6ed1b19%3A0xa2c2519e77bcdf0e!2sAlayl%C4%B1+Ticaret!5e0!3m2!1str!2str!4v1499980832792" width="100%" height="450" class="m-t-b-30"  frameborder="0" style="border:0" allowfullscreen></iframe>
          
            </div>
            <div class="p-30">
            <h2>İLETİŞİM FORMU</h2>
            <div class="col-md-4 col-xs-12">
                
                   <input type="text" class="form-control" name="name" placeholder="Ad Soyad"/>
                    <input type="telephone" class="form-control" name="telefon" placeholder="Telefon"/>
                    <input type="email" class="form-control" name="email" placeholder="E-Mail"/>
                           
            </div>
            <div class="col-md-8 col-xs-12">
                <textarea name="mesaj" class="form-control" placeholder="Mesajınız"></textarea>
                <asp:Button Id="btnSend" Css="btn pull-right" runat="server" Text="Gönder"></asp:Button>
                            
            </div>
            </div>
        </div>

    </div>

</asp:Content>

