﻿using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web;
using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;
using wojilu.sample.Domain;

namespace wojilu.sample.Controller.Admin {

    public class FooterController : ControllerBase {

        public void Index() {
            set( "addLink", to( Add ) );
            List<Footer> list = cdb.findAll<Footer>();
            bindList( "list", "footer", list, bindLink );
        }

        private void bindLink( IBlock block, long id ) {
            block.Set( "footer.EditLink", to( Edit, id ) );
            block.Set( "footer.DeleteLink", to( Delete, id ) );
        }

        public void Add() {
            target( Create );
            editor( "footer.Content", "", "300px" );
        }

        [HttpPost, DbTransaction]
        public void Create() {

            Footer f = ctx.PostValue<Footer>();
            f.Content = ctx.PostHtml( "footer.Content" );
            validateInput( f );

            if (ctx.HasErrors) {
                run( Add );
                return;
            }

            cdb.insert( f );
            echoRedirect( "添加成功", Index );
        }


        public void Edit( long id ) {
            target( Update, id );
            Footer f = cdb.findById<Footer>( id );
            bind( f );
            editor( "footer.Content", f.Content, "300px" );
        }

        [HttpPost, DbTransaction]
        public void Update( long id ) {
            Footer f = cdb.findById<Footer>( id );
            f = ctx.PostValue( f ) as Footer;
            f.Content = ctx.PostHtml( "footer.Content" );

            validateInput( f );

            if (ctx.HasErrors) {
                run( Edit, id );
                return;
            }

            cdb.update( f );
            redirect( Index );
        }

        [HttpDelete, DbTransaction]
        public void Delete( long id ) {
            Footer f = cdb.findById<Footer>( id );
            cdb.delete( f );
            redirect( Index );
        }

        private void validateInput( Footer f ) {
            if (strUtil.IsNullOrEmpty( f.Name )) errors.Add( "请填写页脚名称" );
            if (strUtil.IsNullOrEmpty( f.Content )) errors.Add( "请填写内容" );
        }
    }

}
