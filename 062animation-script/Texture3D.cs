using OpenTK;
using Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JaroslavNejedly
{

  public abstract class Texture3D : ITexture
  {
    public virtual Func<Intersection, Vector3d> Mapping { get; set; }

    public abstract long Apply (Intersection inter);
  }
}
