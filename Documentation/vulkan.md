# VULKAN PRINCIPE VORKFOW

Graphics.Device
    Init
        Instance , Debug , Surface
        Physical device , logic device
        optimal information     => Device_OptimalValues and Device_Informations in data

    Dispose

Graphics.Render
    Init
        SwapChain , depth buffer  => For Image View  
            internal RecreateSwapChain need curent framebuffer
        Command Pool , Command buffers
        Synchronization cache control => Fence , Semaphore

    Draw
        PREPARE A FRAME
        SYNCHRONISATION OBJECTS :  USED TO CONTROL THE ORDER OF EXECUTION OF ASYNCHRONOUS OPERATION
        ACQUIRE NEXT IMAGE 
        UPDATE BUFFERS =>  see UpdatePipeline
      
        // RECORD COMMANDS WICH CAN THEN BE SUBMITED TO DEVICE QUEUE FOR EXECUTION :
        BEGIN RECORD COMMAND BUFFER => Start Thread
            Foreach Render Pass ( set render pass : framebuffers, color, surface, ...)
            DRAW PIPELINE  => Do Build/Compute Before 
                FOREACH SHADER =>    Set shdar state ( shader tesslation) : pushconstant, BindDescriptors = SEND DATA To SHADER
                    FOREACH MATERIALS => Set material state ( texture, uniform, sampler )
                        FOREACH GEOMETRY => Set object state (buffers vertex indice matrices) : BindVertex, BindIndices
                            DRAW CALL
                        END GEOMETRY
                    END MATERIALS
                END SHADER
            END RENDER PASS
        END COMMAND BUFFER
        SUBMIT GRAPHICS COMMAND BUFFERS        
        PRESENT IMAGE        

    Dispose

Graphics.Scene/Pipeline  
    Load
        convert ResourceFile into graphic pipeline data

    Build/Compute
        Framebuffer => need image view 
        RenderPass => need framebuffers
        
        // If pipeline empty don't do following lines
        CreateVertexIndiceBuffer
        CreateTextures  CreateTextureImageView CreateTextureSampler
        CreateUniformBuffers
        DescriptorSetLayout  DescriptorPool DescriptorsSet UpdateDescriptorsSet  CreatePipelineLayout
        CreatePipeline => Setstates(piepline layout, rasterization, dynamic, inputassembly , shader, ...)

    Update
        update buffers : uniform sampler, matrix ,vertex , ....

    Dispose
/////////////////////////////
Geometry

NEED FOR RESOURCE CREATION =>
    CreateVertexBuffer => VertexBuffer + VertexBufferMemory
    CreateIndiceBuffer => IndicesBuffer + IndicesBufferMemory

NEED FOR SHADER  / PIPELINE  =>
    bindingDescription + attributeDescriptions + inputAssembly

NEED FOR LOOP  DRAW CALL To BIND VERTEX AND INDICES
    offsets , indiceSize, NumberVertexToDraw,  VertexBufer + IndiceBuffer, ....

///////////////////////////
Texture / MAterials

NEED FOR RESOURCE CREATION => need for memory barrier , command buffer , create ImageView ...
    Create Texture Buffer =>    TextureImage + TextureImageMemory

NEED FOR RESOURCE DESCRIPTOR => For Descriptor DESCRIPTOR SET =>  befor shader
    textureIMageView => need valid TextureImage
    textureSampler   =>need to declare   VkDescriptorSetLayoutBinding

//////////////////////////////
Shader  / pipeline

NEED FOR RESOURCE CREATION
    Uniform buffer

NEED FOR RESOURCE DESCRIPTOR
    DescriptorSet + layout => texturesSampler, textureImageView , uniform , ....
    CreateDescriptor => pipelinelayout : need  PushConstant , DescriptorSetLayout

FINAL CREATE PIPELINE
    CreatePipeline  => States ( dynamic, geometryState, viewport , rasterization, render pass , pipeline layout = descriptor   )

NEED FOR LOOP
    Bind Pipline
    updateDynamicState 'one per shader + updateuniform ( one per frame)
    Draw call offsets , indiceSize, NumberVertexToDraw,  VertexBufer + IndiceBuffer, .... (1 per object ) + UpdatePushConstant (1 per object )

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
Scene :
if use camera  declare RESOURCE CREATION ( buffer) ,RESOURCE DESCRIPTOR ( layout )  Update ( copyblock )

Add object
    Select type Geometry ( Position Normal Color textureCoordonnee ) RESOURCE DESCRIPTOR
    REsource Creation => indice buffer vertex buffer
    Resource Creation Textures? =materials ?
    Resource Creation TexturesNormal? =materials ?
    PushConstant ( MAtrix )
    CreateShader ( Pipeline )

----------------------------------------------------------------------------------------------------------------------------------------------
CONFIG :
    object_Geometry = file.obj
    object_Trasnform = position
    object_images = files[]
    Camera = Camera (projection ,view)
    PipelineStates = PipelineStateDefault
DATA :
Object
    Geometry_VertexFormat ( postionNormlcollorTexCoord2D , positionTex , ...)
    Geometry_IndiceFormat (int , uint , short)
    Geometry_VertexData => float[]
    Geometry_IndicesData => short[]
    Geometry_DrawVertexCount
    Geometry_DrawOffset
    Geometry_VertexBuffer
    Geometry_IndiceBuffer
    Geometry_IndiceMemory
    Geometry_VertexMemory
    Geometry_PushConstantMatrixModel
    Geometry_TextureBuffer
    Geometry_TextureBufferMemory
    Geometry_TextureBufferView
    Geometry_TextureWidth
    Geometry_TextureHeight
    Geometry_TextureFormat
    Geometry_TextureMimapLevel
    Geometry_TextureSampler
    Geometry_Texture
Camera
    Camer_UniformBuffer
    Camer_UniformBufferMemory
    Camera_Data =>
Pipeline
    States create
