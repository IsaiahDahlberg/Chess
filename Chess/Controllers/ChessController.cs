using Logic;
using Logic.MovementLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Chess.Controllers
{
    [RoutePrefix("api/chess")]
    public class ChessController : ApiController
    {
        private Board _board = new Board(LogicFactory.CreateIMoveChecker(), LogicFactory.CreateICheckChecker());

        [Route("Move/{id:int}/{x:int}/{y:int}")]
        [HttpGet]
        public IHttpActionResult Move(int id, int x, int y)
        {
            _board.MakeMove(id, x, y);
            return Ok(_board.Grid);
        }

        [Route("getgrid")]
        public IHttpActionResult GetGrid()
        {
            return Ok(_board.Grid);
        }

        [Route("setBoard")] 
        [HttpGet]
        public IHttpActionResult SetBoard()
        {
            _board.SetBoard();
            return Ok(_board.Grid);
        }
    }
}
