sealed abstract class ColumnIndex(i: Int) {
  def toInt: Int = i
}

case object CI_1 extends ColumnIndex(0)
case object CI_2 extends ColumnIndex(1)
case object CI_3 extends ColumnIndex(2)
case object CI_4 extends ColumnIndex(3)
case object CI_5 extends ColumnIndex(4)
case object CI_6 extends ColumnIndex(5)
case object CI_7 extends ColumnIndex(6)

sealed abstract class Slot(str: String) {
  override def toString: String = str
}

case object Empty extends Slot(" ")

sealed abstract class Coin(str: String) extends Slot(str)
case object Red extends Coin("0")
case object Yellow extends Coin("1")

sealed abstract class GameStatus(str: String) {
  override def toString: String = str
}

case object NotFinish extends GameStatus("Not Finish")
case object Draw extends GameStatus("Draw")
case class Winner(coin: Coin) extends GameStatus(s"Winner : $coin")
case class ColumnFull(i: ColumnIndex) extends GameStatus(s"Column $i is Full")
/*
sealed trait Path
sealed trait WinPath
case object Unknown extends WinPath
case object RedWin extends WinPath
case object YellowWin extends WinPath

case class Root(a: Path, b: Path, c: Path, d: Path, e: Path, f: Path, g: Path)
case class Node(w: WinPath, wr: Int, a: Node, b: Node, c: Node, d: Node, e: Node, f: Node, g: Node) extends Path
case object Stalemate extends Path
*/
class Board(private var coin: Coin) {
  private var _count = 0
  private val coinOrder = Array.ofDim[Int](2, 21)

  private case class Column(var a: Slot, var b: Slot, var c: Slot, var d: Slot, var e: Slot, var f: Slot) {
    def apply(i: Int): Slot = i match {
      case 0 => a; case 1 => b; case 2 => c; case 3 => d; case 4 => e; case 5 => f
    }
  }

  private var cols = {
    def e = Column(Empty, Empty, Empty, Empty, Empty, Empty)
    Array(e, e, e, e, e, e, e)
  }

  override def toString: String = {
    def f(i: Int, j: Int): String = if(i > 6) "|" else "|" + cols(i)(j) + f(i + 1, j)
    def g(i: Int): String = if(i > 4) f(0, i) else f(0, i) + "\n" + g(i + 1)
    g(0)
  }

  def count = _count

  def getOrder(i: Int): Int = coinOrder(i % 2)(i / 2)

  def clear(c: Coin): Board = {
    coin = c
    cols.foreach(col => {col.a = Empty; col.b = Empty; col.c = Empty; col.d = Empty; col.e = Empty; col.f = Empty})
    val x = coinOrder(0)
    val y = coinOrder(1)
    for(i <- 0 to (_count + 1) / 2) {
      x(i) = 0
      y(i) = 0
    }
    _count = 0
    this
  }

  def rearrange: Board = {
    var y = 0
    var j = 0
    var c = _count
    var z = 1

    while (c > 2) {
      c = c - z
      var i = c / 2
      var x = c % 2
      if (x == 0) {
        y = 1
        j = i - 1
      }
      else {
        y = 0
        j = i
      }

      var a = coinOrder(x)
      var b = coinOrder(y)
      var v = a(i)
      var i2 = i - 1

      if (i > 0 && a(i) < a(i2) && b(j) != v) {
        z = 0
        do {
          val buff = a(i)
          a(i) = a(i2)
          a(i2) = buff
          i = i2
          i2 = i2 - 1
          j = j - 1
        } while (i > 0 && a(i) < a(i2) && b(j) != v)
      }
      else z = 1
    }
    this
  }

  def putCoin(col: ColumnIndex): GameStatus = {
    val c = coin
    val ci = col.toInt

    def f(col: Column): (Boolean, Int) = {
      def f(slots: (Slot, Slot, Slot)) = slots match {
          case (`c`, `c`, `c`) => true
          case _ => false
      }

      col match {
        case Column(_:Coin, _, _, _, _, _) => (false, -1)
        case Column(_, _, _, _, _, Empty) => col.f = coin; (false, 5)
        case Column(_, _, _, _, Empty, _) => col.e = coin; (false, 4)
        case Column(_, _, _, Empty, _, _) => col.d = coin; (false, 3)
        case Column(_, _, Empty, a, b, c) => col.c = coin; (f(a, b, c), 2)
        case Column(_, Empty, a, b, c, _) => col.b = coin; (f(a, b, c), 1)
        case Column(Empty, a, b, c, _, _) => col.a = coin; (f(a, b, c), 0)
      }
    }

    val row = f(cols(ci))

    def g(coin: Coin): Option[Winner] = {
      def w(i: Int, j: Int, fi: Int => Int, fj: Int => Int): Int =  {
        val x = fi(i)
        val y = fj(j)
        if(x < 0 || y < 0 || x > 6 || y > 5 || cols(x)(y) != coin) 0 else 1 + w(x, y, fi, fj)
      }
      val f = w(ci, row._2, _:Int => Int, _:Int => Int)
      val inc = (_:Int) + 1
      val dec = (_:Int) - 1
      if (f(inc, identity) + f(dec, identity) > 2 || f(inc, inc) + f(dec, dec) > 2 || f(dec, inc) + f(inc, dec) > 2) Some(Winner(coin))
      else None
    }

    if(row._2 == -1) ColumnFull(col) else {
      coinOrder(_count % 2)(_count / 2) = ci
      _count = _count + 1
      val coinBuff = coin
      coin = if (coin == Red) Yellow else Red
      if(row._1 == true) Winner(coinBuff) else g(coinBuff) match {
        case Some(x) => x
        case None => if(_count == 42) Draw else NotFinish
      }
    }
  }
}

object Main {
  def main(args: Array[String]): Unit = {
    var running = true
    var res: GameStatus = NotFinish
    val b = new Board(Red)

    do {
      println()
      res match {
        case Winner(x) => {
          b.clear(Red)
          println(res)
        }
        case _ =>
      }
      print("Order : ")
      for(i <- 0 to b.count - 1) print(b.getOrder(i) + 1)
      println()
      println(b)
      print("Column : ")
      val str = scala.io.StdIn.readLine()
      if(str == "") -1 else str.toInt match {
        case 0 => running = false
        case 1 => res = b.putCoin(CI_1)
        case 2 => res = b.putCoin(CI_2)
        case 3 => res = b.putCoin(CI_3)
        case 4 => res = b.putCoin(CI_4)
        case 5 => res = b.putCoin(CI_5)
        case 6 => res = b.putCoin(CI_6)
        case 7 => res = b.putCoin(CI_7)
        case 123 => b.clear(Red)
        case _ =>
      }
      b.rearrange
    } while(running)
  }
}