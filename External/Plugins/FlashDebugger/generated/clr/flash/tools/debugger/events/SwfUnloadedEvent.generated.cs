//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by jni4net. See http://jni4net.sourceforge.net/ 
//     Runtime Version:2.0.50727.5472
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace flash.tools.debugger.events {
    
    
    #region Component Designer generated code 
    [global::net.sf.jni4net.attributes.JavaClassAttribute()]
    public partial class SwfUnloadedEvent : global::flash.tools.debugger.events.DebugEvent {
        
        internal new static global::java.lang.Class staticClass;
        
        internal static global::net.sf.jni4net.jni.FieldId _id0;
        
        internal static global::net.sf.jni4net.jni.FieldId _index1;
        
        internal static global::net.sf.jni4net.jni.FieldId _path2;
        
        internal static global::net.sf.jni4net.jni.FieldId _information3;
        
        internal static global::net.sf.jni4net.jni.MethodId @__ctorSwfUnloadedEvent4;
        
        [global::net.sf.jni4net.attributes.JavaMethodAttribute("(JLjava/lang/String;I)V")]
        public SwfUnloadedEvent(long par0, global::java.lang.String par1, int par2) : 
                base(((global::net.sf.jni4net.jni.JNIEnv)(null))) {
            global::net.sf.jni4net.jni.JNIEnv @__env = global::net.sf.jni4net.jni.JNIEnv.ThreadEnv;
            using(new global::net.sf.jni4net.jni.LocalFrame(@__env, 16)){
            @__env.NewObject(global::flash.tools.debugger.events.SwfUnloadedEvent.staticClass, global::flash.tools.debugger.events.SwfUnloadedEvent.@__ctorSwfUnloadedEvent4, this, global::net.sf.jni4net.utils.Convertor.ParPrimC2J(par0), global::net.sf.jni4net.utils.Convertor.ParStrongCp2J(par1), global::net.sf.jni4net.utils.Convertor.ParPrimC2J(par2));
            }
        }
        
        protected SwfUnloadedEvent(global::net.sf.jni4net.jni.JNIEnv @__env) : 
                base(@__env) {
        }
        
        public static global::java.lang.Class _class {
            get {
                return global::flash.tools.debugger.events.SwfUnloadedEvent.staticClass;
            }
        }
        
        [global::net.sf.jni4net.attributes.JavaMethodAttribute("J")]
        public virtual long id {
            get {
                global::net.sf.jni4net.jni.JNIEnv @__env = this.Env;
            using(new global::net.sf.jni4net.jni.LocalFrame(@__env, 10)){
                return ((long)(@__env.GetLongField(this, global::flash.tools.debugger.events.SwfUnloadedEvent._id0)));
            }
            }
        }
        
        [global::net.sf.jni4net.attributes.JavaMethodAttribute("I")]
        public virtual int index {
            get {
                global::net.sf.jni4net.jni.JNIEnv @__env = this.Env;
            using(new global::net.sf.jni4net.jni.LocalFrame(@__env, 10)){
                return ((int)(@__env.GetIntField(this, global::flash.tools.debugger.events.SwfUnloadedEvent._index1)));
            }
            }
        }
        
        [global::net.sf.jni4net.attributes.JavaMethodAttribute("Ljava/lang/String;")]
        public virtual global::java.lang.String path {
            get {
                global::net.sf.jni4net.jni.JNIEnv @__env = this.Env;
            using(new global::net.sf.jni4net.jni.LocalFrame(@__env, 10)){
                return global::net.sf.jni4net.utils.Convertor.StrongJ2CpString(@__env, @__env.GetObjectFieldPtr(this, global::flash.tools.debugger.events.SwfUnloadedEvent._path2));
            }
            }
        }
        
        [global::net.sf.jni4net.attributes.JavaMethodAttribute("Ljava/lang/String;")]
        public virtual global::java.lang.String information {
            get {
                global::net.sf.jni4net.jni.JNIEnv @__env = this.Env;
            using(new global::net.sf.jni4net.jni.LocalFrame(@__env, 10)){
                return global::net.sf.jni4net.utils.Convertor.StrongJ2CpString(@__env, @__env.GetObjectFieldPtr(this, global::flash.tools.debugger.events.SwfUnloadedEvent._information3));
            }
            }
        }
        
        private static void InitJNI(global::net.sf.jni4net.jni.JNIEnv @__env, java.lang.Class @__class) {
            global::flash.tools.debugger.events.SwfUnloadedEvent.staticClass = @__class;
            global::flash.tools.debugger.events.SwfUnloadedEvent._id0 = @__env.GetFieldID(global::flash.tools.debugger.events.SwfUnloadedEvent.staticClass, "id", "J");
            global::flash.tools.debugger.events.SwfUnloadedEvent._index1 = @__env.GetFieldID(global::flash.tools.debugger.events.SwfUnloadedEvent.staticClass, "index", "I");
            global::flash.tools.debugger.events.SwfUnloadedEvent._path2 = @__env.GetFieldID(global::flash.tools.debugger.events.SwfUnloadedEvent.staticClass, "path", "Ljava/lang/String;");
            global::flash.tools.debugger.events.SwfUnloadedEvent._information3 = @__env.GetFieldID(global::flash.tools.debugger.events.SwfUnloadedEvent.staticClass, "information", "Ljava/lang/String;");
            global::flash.tools.debugger.events.SwfUnloadedEvent.@__ctorSwfUnloadedEvent4 = @__env.GetMethodID(global::flash.tools.debugger.events.SwfUnloadedEvent.staticClass, "<init>", "(JLjava/lang/String;I)V");
        }
        
        new internal sealed class ContructionHelper : global::net.sf.jni4net.utils.IConstructionHelper {
            
            public global::net.sf.jni4net.jni.IJvmProxy CreateProxy(global::net.sf.jni4net.jni.JNIEnv @__env) {
                return new global::flash.tools.debugger.events.SwfUnloadedEvent(@__env);
            }
        }
    }
    #endregion
}
